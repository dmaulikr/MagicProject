using UnityEngine;
using System.Collections.Generic;

[RequireComponent( typeof(Animator) )]
[RequireComponent( typeof(SlimeActions), typeof(SlimeAnimation) )]
public class Slime : MonoBehaviour {

    private SlimeActions action_;
    private SlimeAnimation animation_;

    void Start()
    {
        action_ = GetComponent<SlimeActions>();
        animation_ = GetComponent<SlimeAnimation>();

        action_.addObserver(animation_);
        animation_.addObserver(action_);
    }
}
