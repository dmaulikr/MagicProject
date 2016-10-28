using UnityEngine;
using System.Collections;

public interface IGameActor
{
    void Move(float vert, float horz);
    void Attack();
}
