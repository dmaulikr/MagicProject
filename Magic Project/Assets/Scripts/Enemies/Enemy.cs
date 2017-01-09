using System;

public class Enemy : Subject {
    public event Action<IGameActor> deathEvent;
    protected void ExecuteDeathEvent(IGameActor actor) { if(deathEvent != null) deathEvent(actor); }
}
