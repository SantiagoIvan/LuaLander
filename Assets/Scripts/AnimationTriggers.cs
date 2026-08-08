using UnityEngine;
using System.Collections.Generic;
public enum AnimatorTrigger
{
    Flash,
    ShowIndicator,
}

public static class AnimatorTriggerExtensions
{
    private static readonly Dictionary<AnimatorTrigger, int> hashes = new Dictionary<AnimatorTrigger, int>
    {
        { 
            AnimatorTrigger.Flash, Animator.StringToHash(nameof(AnimatorTrigger.Flash))
        },
        { 
            AnimatorTrigger.ShowIndicator, Animator.StringToHash(nameof(AnimatorTrigger.ShowIndicator))
        }
    };

    public static void SetTrigger(this Animator animator, AnimatorTrigger trigger)
    {
        animator.SetTrigger(hashes[trigger]);
    }
}