using UnityEngine;

public class SlimeActions : Enemy, IGameActor, IObserver {

    [SerializeField]
    private int initialHealth_;
    [SerializeField]
    private int defense_;

    private int health_;

    public void Start()
    {
        health_ = initialHealth_;
    }

    public void FixedUpdate()
    {
        if (health_ <= 0) notify(this, Event.EVENT_ACTOR_DIE);
    }

    public void move(float vert, float horz)
    {

    }
    public void attack()
    {

    }
    public void useBuff(IGameActor entity, Buff buff)
    {

    }
    public void takeDamage(int damage)
    {
        health_ -= damage;
        if (health_ > 0)
            notify(this, Event.EVENT_ACTOR_TAKE_DAMAGE);
    }

    private void Die()
    {
        Debug.Log("Ded");
        ExecuteDeathEvent(this);
        Destroy(gameObject);
    }

    public void onNotify(IGameActor actor, Event ev)
    {
        switch (ev)
        {
            case Event.EVENT_ACTOR_DIE_ANIM_ENDED:
                Die();
                break;
        }
    }

}
