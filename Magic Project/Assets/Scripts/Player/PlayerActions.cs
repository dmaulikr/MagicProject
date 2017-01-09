using UnityEngine;
using System;

public class PlayerActions : Subject, IObserver, IGameActor {

    [SerializeField]
    private float speed_;
    [SerializeField]
    private float attackDistance_;
    [SerializeField]
    private int attackDamage_;
    [SerializeField]
    private LayerMask enemyLayer_;
    [SerializeField]
    private Vector2 attackColliderOffset_;
    [SerializeField]
    private Vector2 attackColliderSize_;
    [SerializeField]
    private float interactionDist_;
    [SerializeField]
    private LayerMask interactableLayer_;

    private Rigidbody2D rb2D_;

    private bool facingRight = true;
    private bool canAct = true;
    private Vector3 lastPos;
    private float lastPosThreshhold = 0.01f;
    private bool isHurting = false;

    public void Start()
    {
        rb2D_ = GetComponent<Rigidbody2D>();

        PlayerAnimation anim = GetComponent<PlayerAnimation>();
        anim.addObserver(this);
    }

    public void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, lastPos) > lastPosThreshhold) notify(null, Event.EVENT_ACTOR_MOVE);
        else notify(null, Event.EVENT_ACTOR_STOP);

        lastPos = transform.position;
    }

    #region IGameActor :: move
    public void move(float vert, float horz)
    {
        if (!canAct) rb2D_.velocity = Vector2.zero;
        else {
            rb2D_.velocity = Vector3.Normalize(vert * Vector2.up + horz * Vector2.right) * speed_;
            Flip(horz);
        }
    }
    private void Flip(float horz)
    {
        if (facingRight && horz < 0 || !facingRight && horz > 0)
        {
            facingRight = !facingRight;
            Vector3 s = transform.localScale;
            s.x *= -1f;
            transform.localScale = s;
        }
    }
    #endregion

    public void useBuff(IGameActor actor, Buff buff)
    {
        if (!canAct) return;
        if (buff == Buff.BUFF_HEAL)
        {
            Debug.Log("Healed!");
            notify(null, Event.EVENT_ACTOR_HEAL_BUFF);
            canAct = false;
        }
    }
    public void takeDamage(int damage)
    {
        if (isHurting) return;
        notify(null, Event.EVENT_ACTOR_TAKE_DAMAGE);
        Debug.Log("Ouch!");
        canAct = false;
        isHurting = true;
    }
    
    #region IGameActor :: attack
    public void attack()
    {
        if (!canAct) return;
        notify(null, Event.EVENT_ACTOR_ATTACK);

        var enemiesOnTarget_ = GetEnemiesOnTarget();

        foreach (IGameActor enemy in enemiesOnTarget_)
        {
            if (enemy != null) enemy.takeDamage(attackDamage_);
        }

        canAct = false;
    }
    private IGameActor[] GetEnemiesOnTarget()
    {
        int lookingSide = facingRight ? 1 : -1;
        Vector2 correctedOffset = attackColliderOffset_;
        correctedOffset.x *= lookingSide;

        Vector2 origin = (Vector2)transform.position + correctedOffset;

        var hits = Physics2D.BoxCastAll(origin, attackColliderSize_, 0f, Vector2.zero, Mathf.Infinity, enemyLayer_);
        IGameActor[] enemiesOnTarget = Array.ConvertAll(hits, new Converter<RaycastHit2D, IGameActor>(GetGameActor));

        return enemiesOnTarget;
    }
    private IGameActor GetGameActor(RaycastHit2D hit)
    {
        IGameActor enemy = hit.collider.gameObject.GetComponent<IGameActor>();
        #if UNITY_EDITOR
        if (enemy == null) Error.ShowError("Object with \"Enemy\" tag does not have a IGameActor script");
        #endif
        return enemy;
    }
    #endregion

    public void interact()
    {
        var hit = Physics2D.CircleCast(transform.position + interactionDist_/2 * transform.right, interactionDist_, Vector2.up, Mathf.Infinity, interactableLayer_);
        if (hit.collider == null) return;
        var hitObj = hit.collider.gameObject;
        IInteractable interactable = hitObj.GetComponent<IInteractable>();
        #if UNITY_EDITOR
        if (interactable == null) Error.ShowError("Object with \"Interactable\" tag does not have a IInteractable script");
        #endif
        interactable.interact();
    }

    public void onNotify(IGameActor actor, Event ev)
    {
        switch (ev)
        {
            case Event.EVENT_ACTOR_ATTACK_ANIM_ENDED:
            case Event.EVENT_ACTOR_HEAL_ANIM_ENDED:
                canAct = true;
                break;
            case Event.EVENT_ACTOR_TAKE_DAMAGE_ANIM_ENDED:
                isHurting = false;
                canAct = true;
                break;
        }
    }

    
}
