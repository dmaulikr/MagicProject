using UnityEngine;
using System.Collections;


public class PlayerMovement : IGameActor {


    private float speed_;
    private Rigidbody2D rb2D_;
    private bool facingRight_ = true;
    private Transform transform_;

    public PlayerMovement(Transform transform, float speed, Rigidbody2D rb2D)
    {
        transform_ = transform;
        speed_ = speed;
        rb2D_ = rb2D;
    }

    public void Move(float vert, float horz)
    {
        rb2D_.velocity = (vert * Vector2.up + horz * Vector2.right) * speed_;
        Flip(horz);
    }

    public void Attack()
    {
        Debug.Log("Pow!!");
    }

    void Flip(float horz)
    {
        if(facingRight_ && horz < 0 ||!facingRight_ && horz > 0)
        {
            facingRight_ = !facingRight_;
            Vector3 s = transform_.localScale;
            s.x *= -1f;
            transform_.localScale = s;
        }
    }
}
