public enum Direction
{
    DIR_UP, DIR_DOWN, DIR_LEFT, DIR_RIGHT,
    DIR_UP_LEFT, DIR_UP_RIGHT, DIR_DOWN_LEFT, DIR_DOWN_RIGHT
};

public interface ICommand
{
    void execute();
}

public class MoveCommand : ICommand
{
    private IGameActor actor_;
    private float vert_;
    private float horz_;

    public MoveCommand(IGameActor actor, float vertMove, float horzMove)
    {
        actor_ = actor; 
        vert_ = vertMove;
        horz_ = horzMove;
    }

    public void execute()
    {
        actor_.move(vert_, horz_);
    }
}
public class AttackCommand : ICommand
{
    private IGameActor actor_;

    public AttackCommand(IGameActor actor)
    {
        actor_ = actor;
    }

    public void execute()
    {
        actor_.attack();
    }
}
public class BuffCommand : ICommand
{
    private IGameActor actor_;
    private Buff buff_;

    public BuffCommand(IGameActor actor, Buff buff)
    {
        actor_ = actor;
        buff_ = buff;
    }

    public void execute()
    {
        actor_.useBuff(actor_, buff_);
    }
}
public class InteractCommand : ICommand
{
    private IGameActor actor_;

    public InteractCommand(IGameActor actor)
    {
        actor_ = actor;
    }

    public void execute()
    {
        actor_.interact();
    }
}