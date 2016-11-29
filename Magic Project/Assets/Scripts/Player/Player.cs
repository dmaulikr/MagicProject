using UnityEngine;
using System.Collections.Generic;

[RequireComponent( typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer) )]
[RequireComponent( typeof(PlayerActions), typeof(PlayerAnimation), typeof(InputHandler) )]
public class Player : MonoBehaviour {

    private PlayerActions action_;
    private PlayerAnimation animation_;
    private InputHandler input_;

    void Start ()
    {
        action_ = GetComponent<PlayerActions>();
        animation_ = GetComponent<PlayerAnimation>();

        action_.addObserver(animation_);
        animation_.addObserver(action_);
	}
}
