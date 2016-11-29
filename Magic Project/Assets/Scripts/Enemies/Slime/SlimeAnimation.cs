using UnityEngine;
using System.Collections;

public class SlimeAnimation : Subject, IObserver {

    private Animator animator_;
    private IGameActor slimeActor_;

    private AnimationController animController;

    private bool isHurting = false;
    private bool isDying = false;

    public void Start()
    {
        animator_ = GetComponent<Animator>();
        animController = new AnimationController(animator_);
        slimeActor_ = GetComponent<IGameActor>();
    }

    public void FixedUpdate()
    {
        NotifyInAnimationEnd(ref isHurting, "slime_hurt", Event.EVENT_ACTOR_TAKE_DAMAGE_ANIM_ENDED);
        NotifyInAnimationEnd(ref isDying, "slime_death", Event.EVENT_ACTOR_DIE_ANIM_ENDED);
    }

    void NotifyInAnimationEnd(ref bool isAnimating, string stateName, Event notification)
    {
        if (animController.HasAnimationEnded(ref isAnimating, stateName))
            notify(null, notification);
    }

    public void onNotify(IGameActor actor, Event ev)
    {
        switch (ev)
        {
            case Event.EVENT_ACTOR_TAKE_DAMAGE:
                if (actor == slimeActor_ && !isHurting) animator_.SetTrigger("hurt");
                isHurting = true;
                break;
            case Event.EVENT_ACTOR_DIE:
                if(actor == slimeActor_ && !isDying) animator_.SetTrigger("die");
                isDying = true;
                break;
        }
    }
}
