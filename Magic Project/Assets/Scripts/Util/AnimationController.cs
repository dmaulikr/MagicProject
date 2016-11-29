using UnityEngine;

public class AnimationController
{
    private Animator animator_;

    public AnimationController(Animator animator)
    {
        animator_ = animator;
    }

    bool AnimatorIsPlaying()
    {
        return animator_.GetCurrentAnimatorStateInfo(0).length >
               animator_.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && animator_.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public bool HasAnimationEnded(ref bool isAnimating, string stateName)
    {
        if (isAnimating && !AnimatorIsPlaying(stateName))
        {
            isAnimating = false;
            return true;
        }
        return false;
    }
}
