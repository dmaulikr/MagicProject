using UnityEngine;
using System.Collections;

public class Enemy : Subject {
    public delegate void DeathDelegate(IGameActor actor);
    public event DeathDelegate deathEvent;

    protected void ExecuteDeathEvent(IGameActor actor) { deathEvent(actor); }
}
