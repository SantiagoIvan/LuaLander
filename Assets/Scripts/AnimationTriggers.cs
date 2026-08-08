using UnityEngine;
using System.Collections.Generic;
public enum AnimatorTrigger
{
    Flash,
    Collect,
}

public static class AnimatorTriggerExtensions
{
    private static readonly Dictionary<AnimatorTrigger, int> hashes = new Dictionary<AnimatorTrigger, int>
    {
        { 
            AnimatorTrigger.Flash, Animator.StringToHash(nameof(AnimatorTrigger.Flash))
        },
        { 
            AnimatorTrigger.Collect, Animator.StringToHash(nameof(AnimatorTrigger.Collect))
        }
    };

    public static void SetTrigger(this Animator animator, AnimatorTrigger trigger)
    {
        animator.SetTrigger(hashes[trigger]);
    }
}