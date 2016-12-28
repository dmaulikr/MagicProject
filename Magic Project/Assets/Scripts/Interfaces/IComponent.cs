using UnityEngine;

public interface IComponent {
    void update();
}

public interface PhysicsComponent : IComponent
{
   void onCollisionEnter(Collision2D coll);
   void onCollisionStay(Collision2D coll);
   void onCollisionExit(Collision2D coll);
}