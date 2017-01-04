using UnityEngine;

public class PlayerAnimation : Subject, IObserver
{
    private Animator animator_;
    private bool isAttacking = false;
    private bool isHealing = false;
    private bool isMoving = false;
    private bool isHurting = false;


    private AnimationController animController;

    void Start()
    {
        animator_ = GetComponent<Animator>();
        animController = new AnimationController(animator_);
    }

    void FixedUpdate()
    {
        animator_.SetBool("move", isMoving);

        NotifyInAnimationEnd(ref isAttacking, "player_attack", Event.EVENT_ACTOR_ATTACK_ANIM_ENDED);
        NotifyInAnimationEnd(ref isHealing, "player_heal", Event.EVENT_ACTOR_HEAL_ANIM_ENDED);
        NotifyInAnimationEnd(ref isHurting, "player_hurt", Event.EVENT_ACTOR_TAKE_DAMAGE_ANIM_ENDED);

    }

    void NotifyInAnimationEnd(ref bool isAnimating, string stateName, Event notification)
    {
        if(animController.HasAnimationEnded(ref isAnimating, stateName))
            notify(null, notification);
    }

    public void onNotify(IGameActor actor, Event ev)
    {
        switch(ev)
        {
            case Event.EVENT_ACTOR_MOVE:
                isMoving = true;
                break;
            case Event.EVENT_ACTOR_STOP:
                isMoving = false;
                break;
            case Event.EVENT_ACTOR_ATTACK:
                animator_.SetTrigger("attack");
                isAttacking = true;
                break;
            case Event.EVENT_ACTOR_HEAL_BUFF:
                animator_.SetTrigger("heal");
                isHealing = true;
                break;
            case Event.EVENT_ACTOR_TAKE_DAMAGE:
                animator_.SetTrigger("hurt");
                isHurting = true;
                break;
        }
    }
}
