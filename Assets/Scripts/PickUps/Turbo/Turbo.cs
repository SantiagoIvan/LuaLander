using UnityEngine;
using System;

public class Turbo : PickUpReward
{
    override protected void apply()
    {
        Lander.Instance.GetTurboMeter().Add(this.amount);
        Lander.Instance.RaiseOnTurboCollectedEvent();
    }
}
