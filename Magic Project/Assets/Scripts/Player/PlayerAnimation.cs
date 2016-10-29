using UnityEngine;
using System.Collections;

public class PlayerAnimation : Subject, IComponent, IObserver
{
    private Animator animator_;
    private Rigidbody2D rb2D_;
    private bool isAttacking = false;
    private bool isHealing = false;

    public PlayerAnimation(Animator anim, Rigidbody2D rb2D)
    {
        animator_ = anim;
        rb2D_ = rb2D;
    }

    public void update()
    {
        animator_.SetFloat("vel", Mathf.Abs(rb2D_.velocity.x) + Mathf.Abs(rb2D_.velocity.y));

        NotifyInAnimationEnd(ref isAttacking, "player_attack", Event.EVENT_ACTOR_ATTACK_ANIM_ENDED);
        NotifyInAnimationEnd(ref isHealing, "player_heal", Event.EVENT_ACTOR_HEAL_ANIM_ENDED);
    }

    bool AnimatorIsPlaying()
    {
        return animator_.GetCurrentAnimatorStateInfo(0).length >
               animator_.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && animator_.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    void NotifyInAnimationEnd(ref bool isAnimating, string stateName, Event notification)
    {
        if(isAnimating && !AnimatorIsPlaying(stateName))
        {
            isAnimating = false;
            notify(null, notification);
        }
    }

    public void onNotify(IGameActor actor, Event ev)
    {
        switch(ev)
        {
            case Event.EVENT_ACTOR_ATTACK:
                animator_.SetTrigger("attack");
                isAttacking = true;
                break;
            case Event.EVENT_ACTOR_HEAL_BUFF:
                animator_.SetTrigger("heal");
                isHealing = true;
                break;
        }
    }
}
