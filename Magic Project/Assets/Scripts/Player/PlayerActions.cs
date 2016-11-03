using UnityEngine;


public class PlayerActions : Subject, IObserver, IGameActor {

    private float speed_;
    private Rigidbody2D rb2D_;
    private bool facingRight_ = true;
    private bool canAct = true;
    private Transform transform_;
    private Vector3 lastPos;
    private float lastPosThreshhold = 0.01f;

    public PlayerActions(Transform transform, float speed, Rigidbody2D rb2D)
    {
        transform_ = transform;
        speed_ = speed;
        rb2D_ = rb2D;
    }

    public void fixedUpdate()
    {
        if (Vector2.Distance(transform_.position, lastPos) > lastPosThreshhold) notify(null, Event.EVENT_ACTOR_MOVE);
        else notify(null, Event.EVENT_ACTOR_STOP);

        lastPos = transform_.position;
    }

    public void Move(float vert, float horz)
    {
        if (!canAct) rb2D_.velocity = Vector2.zero;
        else {
            rb2D_.velocity = (vert * Vector2.up + horz * Vector2.right) * speed_;
            Flip(horz);
        }
    }
    public void Attack()
    {
        if (!canAct) return;
        Debug.Log("Pow!!");
        notify(null, Event.EVENT_ACTOR_ATTACK);
        canAct = false;
    }
    public void UseBuff(IGameActor actor, Buff buff)
    {
        if (!canAct) return;
        if (buff == Buff.BUFF_HEAL)
        {
            Debug.Log("Healed!");
            notify(null, Event.EVENT_ACTOR_HEAL_BUFF);
            canAct = false;
        }
    }

    public void onNotify(IGameActor actor, Event ev)
    {
        switch (ev)
        {
            case Event.EVENT_ACTOR_ATTACK_ANIM_ENDED:
            case Event.EVENT_ACTOR_HEAL_ANIM_ENDED:
                canAct = true;
                break;
        }
    }

    void Flip(float horz)
    {
        if (facingRight_ && horz < 0 || !facingRight_ && horz > 0)
        {
            facingRight_ = !facingRight_;
            Vector3 s = transform_.localScale;
            s.x *= -1f;
            transform_.localScale = s;
        }
    }
}
