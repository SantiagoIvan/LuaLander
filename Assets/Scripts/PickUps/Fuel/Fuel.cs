using UnityEngine;
using System;

public class Fuel : PickUpReward 
{
    override protected void apply()
    {
        Lander.Instance.GetFuelMeter().Add(this.amount);
        Lander.Instance.RaiseOnFuelCollectedEvent();

    }
}
