using UnityEngine;
using System;
using TMPro;

public class LowFuelUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI lowFuelTextMeshPro;
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
        this.lowFuelTextMeshPro.text = "LOW FUEL";
        this.updateAnimator();
    }
    private void Lander_OnFuelCollected(object sender, OnFuelCollectedEventArgs e)
    {
        this.updateAnimator();
    }
    private void Lander_OnOutOfFuel(object sender, EventArgs e)
    {
        this.lowFuelTextMeshPro.text = "OUT OF FUEL";
    }
    private void updateAnimator()
    {
        this.animator.SetBool(animatorWarningParameterName, lander.isFuelLow());
    }
}
