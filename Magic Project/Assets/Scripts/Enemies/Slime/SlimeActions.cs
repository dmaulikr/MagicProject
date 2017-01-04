using UnityEngine;

[RequireComponent( typeof(Rigidbody2D) )]
public class SlimeActions : Enemy, IGameActor, IObserver {

    [SerializeField]
    private int initialHealth_;
    [SerializeField]
    private int defense_;
    [SerializeField]
    private float speed_;
    [SerializeField]
    private Transform near_;
    [SerializeField]
    private float nearRadius_ = .5f;
    [SerializeField]
    private LayerMask attackMask_;
    [SerializeField]
    private int lowHealth_;

    private int health_;
    private Rigidbody2D rb2D_;
    private bool facingRight_ = false;

    private Vector3 lastPos;
    private float lastPosThreshhold = 0.01f;

    private bool isHurting = false;

    public void Start()
    {
        health_ = initialHealth_;

        SlimeAnimation animation = GetComponent<SlimeAnimation>();
        animation.addObserver(this);

        rb2D_ = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, lastPos) > lastPosThreshhold) notify(null, Event.EVENT_ACTOR_MOVE);
        else notify(null, Event.EVENT_ACTOR_STOP);

        lastPos = transform.position;

        if (health_ <= 0) notify(this, Event.EVENT_ACTOR_DIE);
        if (health_ <= lowHealth_) notify(this, Event.EVENT_ACTOR_LOW_HEALTH);
    }

    #region IGameActor :: move
    public void move(float vert, float horz)
    {
        rb2D_.velocity = Vector3.Normalize(vert * Vector2.up + horz * Vector2.right) * speed_;
        Flip(horz);
    }
    private void Flip(float horz)
    {
        if (facingRight_ && horz < 0 || !facingRight_ && horz > 0)
        {
            facingRight_ = !facingRight_;
            Vector3 s = transform.localScale;
            s.x *= -1f;
            transform.localScale = s;
        }
    }
    #endregion

    public void attack()
    {
        RaycastHit2D hit = Physics2D.CircleCast(near_.position, nearRadius_, Vector2.up, Mathf.Infinity, attackMask_);
        GameObject player = hit.collider.gameObject;
        IGameActor playerActor = player.GetComponent<IGameActor>();
        #if UNITY_EDITOR
        if (playerActor == null) Error.ShowError("Object with \"Player\" tag does not have a IGameActor script");
#endif

        playerActor.takeDamage(1);

        notify(this, Event.EVENT_ACTOR_ATTACK);
    }
    public void useBuff(IGameActor entity, Buff buff)
    {

    }
    public void takeDamage(int damage)
    {
        if (isHurting) return;
        health_ -= damage;
        if (health_ > 0)
            notify(this, Event.EVENT_ACTOR_TAKE_DAMAGE);
        isHurting = true;
    }

    private void Die()
    {
        ExecuteDeathEvent(this);
        Destroy(gameObject);
    }

    public void onNotify(IGameActor actor, Event ev)
    {
        switch (ev)
        {
            case Event.EVENT_ACTOR_TAKE_DAMAGE_ANIM_ENDED:
                isHurting = false;
                break;
        }
    }

}
