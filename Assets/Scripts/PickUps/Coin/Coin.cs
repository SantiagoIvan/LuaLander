using UnityEngine;

public class Coin : PickUpReward
{
    override protected void apply()
    {
        Debug.Log("Coin collected!");
        Lander.Instance.RaiseOnCoinCollectedEvent(this.getAmount());
    }
}
