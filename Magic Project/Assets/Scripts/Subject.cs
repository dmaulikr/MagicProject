using UnityEngine;
using System.Collections.Generic;

public class Subject : MonoBehaviour
{
    List<IObserver> observers_ = new List<IObserver>();

    public void addObserver(IObserver observer)
    {
        observers_.Add(observer);
    }

    public void removeObserver(IObserver observer)
    {
        observers_.Remove(observer);
    }

    protected void notify(IGameActor actor, Event ev)
    {
        foreach (IObserver ob in observers_)
            ob.onNotify(actor, ev);
    }
}

