public enum Event
{
    EVENT_ACTOR_MOVE, EVENT_ACTOR_STOP, EVENT_ACTOR_ATTACK, EVENT_ACTOR_DIE,
    EVENT_ACTOR_HEAL_BUFF, EVENT_ACTOR_ATTACK_ANIM_ENDED, EVENT_ACTOR_HEAL_ANIM_ENDED,
    EVENT_ACTOR_TAKE_DAMAGE, EVENT_ACTOR_DIE_ANIM_ENDED, EVENT_ACTOR_TAKE_DAMAGE_ANIM_ENDED,
    EVENT_ACTOR_LOW_HEALTH, EVENT_ACTOR_ANIM_PLAYING, EVENT_ACTOR_ANIM_ENDED
};

public enum Buff
{
    BUFF_HEAL
};

public interface IGameActor
{
    void move(float vert, float horz);
    void attack();
    void useBuff(IGameActor entity, Buff buff);
    void takeDamage(int damage);
    void interact();
}

