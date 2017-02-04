using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using IEnumerator = System.Collections.IEnumerator;

public class SlimeAI : MonoBehaviour, IObserver {

    private enum SlimeState
    {
        MOVE_TO_PLAYER, FLEE, IDLE, WALK, CHANGE_DIRECTION, ATTACK, NUMBER_OF_STATES
    };

    [SerializeField]
    private float attackCooldown_;
    [SerializeField]
    private float farFromPlayer_;
    [SerializeField]
    private float nearPlayer_;

    private IGameActor slimeActor_;

    private SlimeState state_;
    private Dictionary<SlimeState, int> stateScore_;
    private Transform player_;

    private bool lowHealth_ = false;
    private bool canAct_ = true;
    private bool cooldown_ = false;

    void Start()
    {
        slimeActor_ = GetComponent<IGameActor>();
        SlimeActions actions = GetComponent<SlimeActions>();
        actions.addObserver(this);
        SlimeAnimation anim = GetComponent<SlimeAnimation>();
        anim.addObserver(this);

        player_ = GameObject.FindGameObjectWithTag("Player").transform;

        stateScore_ = new Dictionary<SlimeState, int>();
        foreach (SlimeState state in Enum.GetValues(typeof(SlimeState)))
            stateScore_.Add(state, 0);
    }

    void Update()
    {
        RecalculateScores();
        state_ = stateScore_.FirstOrDefault(x => x.Value == stateScore_.Values.Max()).Key;
        foreach (SlimeState state in Enum.GetValues(typeof(SlimeState)))
            stateScore_[state] = 0;
        ExecuteAction(state_);
    }

    private void RecalculateScores()
    {
        float distToPlayer = Vector2.Distance(player_.position, transform.position);
        if (distToPlayer > farFromPlayer_)
        {
            stateScore_[SlimeState.FLEE] -= 215;
            stateScore_[SlimeState.MOVE_TO_PLAYER] -= 100;
        }
        else if(distToPlayer > nearPlayer_)
        {
            stateScore_[SlimeState.MOVE_TO_PLAYER] += (int) distToPlayer;
        }
        else
        {
            stateScore_[SlimeState.ATTACK] += 100;
            stateScore_[SlimeState.FLEE] += 15;
            stateScore_[SlimeState.MOVE_TO_PLAYER] -= 20;
        }

        if(lowHealth_ || cooldown_)
        {
            stateScore_[SlimeState.ATTACK] -= 50;
            stateScore_[SlimeState.FLEE] += 50;
            stateScore_[SlimeState.MOVE_TO_PLAYER] -= 120;
        }

    }
    private void ExecuteAction(SlimeState state)
    {
        if (!canAct_)
        {
            slimeActor_.move(0f, 0f);
            return;
        }

        Vector2 distToPlayer;
        switch (state)
        {
            case SlimeState.ATTACK:
                if(!cooldown_) StartCoroutine(Attack());
                slimeActor_.move(0f, 0f);
                break;
            case SlimeState.MOVE_TO_PLAYER:
                distToPlayer = player_.position - transform.position;
                slimeActor_.move(distToPlayer.y, distToPlayer.x);
                break;
            case SlimeState.FLEE:
                distToPlayer = -(player_.position - transform.position);
                slimeActor_.move(distToPlayer.y, distToPlayer.x);
                break;
            default:
                slimeActor_.move(0f, 0f);
                break;
        }
    }

    public void onNotify(IGameActor actor, Event ev)
    {
        switch (ev)
        {
            case Event.EVENT_ACTOR_ANIM_PLAYING:
                canAct_ = false;
                break;
            case Event.EVENT_ACTOR_ANIM_ENDED:
                canAct_ = true;
                break;
            case Event.EVENT_ACTOR_LOW_HEALTH:
                lowHealth_ = true;
                break;
        }

    }

    private IEnumerator Attack()
    {
        cooldown_ = true;
        slimeActor_.attack();
        yield return new WaitForSeconds(attackCooldown_);
        cooldown_ = false;
    }
}
