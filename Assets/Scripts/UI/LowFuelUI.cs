using UnityEngine;
using System;

public class LowFuelUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string animatorWarningParameterName = "IsLowFuel";
    private Lander lander;

    private void Start()
    {
        lander = Lander.Instance;
        lander.OnLowFuel += Lander_OnLowFuel;
        lander.OnFuelCollected += Lander_OnFuelCollected;
        lander.OnOutOfFuel += Lander_OnOutOfFuel;
    }
    private void Lander_OnLowFuel(object sender, EventArgs e)
    {
        this.updateAnimator();
    }
    private void Lander_OnFuelCollected(object sender, OnFuelCollectedEventArgs e)
    {
        this.updateAnimator();
    }
    private void Lander_OnOutOfFuel(object sender, EventArgs e)
    {

    }
    private void updateAnimator()
    {
        Debug.Log("Low fuel is "+ animatorWarningParameterName + " and its value is " + lander.isFuelLow());
        this.animator.SetBool(animatorWarningParameterName, lander.isFuelLow());
    }
}
