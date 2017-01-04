using UnityEngine;
using System.Collections;

public class SlimeAnimation : Subject, IObserver {

    private Animator animator_;
    private IGameActor slimeActor_;

    private AnimationController animController;

    private bool isHurting = false;
    private bool isDying = false;
    private bool isMoving = false;
    private bool isAttacking = false;


    public void Start()
    {
        animator_ = GetComponent<Animator>();
        animController = new AnimationController(animator_);
        slimeActor_ = GetComponent<IGameActor>();

        SlimeActions actions = GetComponent<SlimeActions>();
        actions.addObserver(this);
    }

    public void FixedUpdate()
    {
        animator_.SetBool("move", isMoving);

        NotifyInAnimationEnd(ref isHurting, "slime_hurt", Event.EVENT_ACTOR_TAKE_DAMAGE_ANIM_ENDED);
        NotifyInAnimationEnd(ref isAttacking, "slime_attack", Event.EVENT_ACTOR_ATTACK_ANIM_ENDED);
    }

    void NotifyInAnimationEnd(ref bool isAnimating, string stateName, Event notification)
    {
        if (animController.HasAnimationEnded(ref isAnimating, stateName))
        {
            notify(null, notification);
            if (!isHurting && !isDying) notify(null, Event.EVENT_ACTOR_ANIM_ENDED);
        }
    }

    public void onNotify(IGameActor actor, Event ev)
    {
        switch (ev)
        {
            case Event.EVENT_ACTOR_MOVE:
                isMoving = true;
                break;
            case Event.EVENT_ACTOR_STOP:
                isMoving = false;
                break;
            case Event.EVENT_ACTOR_TAKE_DAMAGE:
                if (actor == slimeActor_)
                {
                    animator_.SetTrigger("hurt");
                    notify(null, Event.EVENT_ACTOR_ANIM_PLAYING);
                }
                isHurting = true;
                break;
            case Event.EVENT_ACTOR_DIE:
                if (actor == slimeActor_ && !isDying)
                {
                    animator_.SetTrigger("die");
                    notify(null, Event.EVENT_ACTOR_ANIM_PLAYING);
                }
                isDying = true;
                break;
            case Event.EVENT_ACTOR_ATTACK:
                if (actor == slimeActor_ && !isAttacking)
                {
                    animator_.SetTrigger("attack");
                    notify(null, Event.EVENT_ACTOR_ANIM_PLAYING);
                }
                isAttacking = true;
                break;
        }
    }
}
