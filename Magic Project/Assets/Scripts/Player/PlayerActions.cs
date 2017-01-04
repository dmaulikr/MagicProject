using UnityEngine;
using System.Collections.Generic;

public class PlayerActions : Subject, IObserver, IGameActor {

    [SerializeField]
    private float speed_;
    [SerializeField]
    private float attackDistance_;
    [SerializeField]
    private LayerMask enemyLayer_;
    [SerializeField]
    private int attackDamage_;

    private float spriteHeight_;
    private Rigidbody2D rb2D_;

    private List<IGameActor> enemiesOnTarget_;

    private bool facingRight = true;
    private bool canAct = true;
    private Vector3 lastPos;
    private float lastPosThreshhold = 0.01f;
    private bool isHurting = false;

    public void Start()
    {
        spriteHeight_ = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        rb2D_ = GetComponent<Rigidbody2D>();

        enemiesOnTarget_ = new List<IGameActor>();
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

        foreach (IGameActor enemy in enemiesOnTarget_)
        {
            if (enemy != null) enemy.takeDamage(attackDamage_);
        }

        canAct = false;
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            IGameActor enemy = col.GetComponent<IGameActor>();
            AddEnemyToTargerList(enemy);
            ((Enemy)enemy).deathEvent += RemoveEnemyFromTargerList;
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            IGameActor enemy = col.GetComponent<IGameActor>();
            RemoveEnemyFromTargerList(enemy);
        }
    }
    private void AddEnemyToTargerList(IGameActor enemy)
    {
        #if UNITY_EDITOR
        if (enemy == null) Error.ShowError("Object with \"Enemy\" tag does not have a IGameActor script");
        #endif
        enemiesOnTarget_.Add(enemy);
    }
    private void RemoveEnemyFromTargerList(IGameActor enemy)
    {
        #if UNITY_EDITOR
        if (enemy == null) Error.ShowError("Object with \"Enemy\" tag does not have a IGameActor script");
        #endif
        enemiesOnTarget_.Remove(enemy);
    }
    #endregion

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
