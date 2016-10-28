using UnityEngine;
using System.Collections;

public class PlayerAnimation : IComponent {

    private Animator animator_;
    private Rigidbody2D rb2D_;

    public PlayerAnimation(Animator anim, Rigidbody2D rb2D)
    {
        animator_ = anim;
        rb2D_ = rb2D;
    }

	public void update()
    {
        animator_.SetFloat("vel", Mathf.Abs(rb2D_.velocity.x) + Mathf.Abs(rb2D_.velocity.y));
    }
}
