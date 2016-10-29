public interface IObserver
{
    void onNotify(IGameActor actor, Event ev);
}