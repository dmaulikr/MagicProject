public enum Event
{
    EVENT_ACTOR_MOVE, EVENT_ACTOR_ATTACK, EVENT_ACTOR_DIE, EVENT_ACTOR_HEAL_BUFF,
    EVENT_ACTOR_ATTACK_ANIM_ENDED, EVENT_ACTOR_HEAL_ANIM_ENDED
};

public enum Buff
{
    BUFF_HEAL
};

public interface IGameActor
{
    void Move(float vert, float horz);
    void Attack();
    void UseBuff(IGameActor entity, Buff buff);
}
