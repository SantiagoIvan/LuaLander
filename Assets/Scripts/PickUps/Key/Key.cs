using UnityEngine;

public class Key : PickUpReward
{
    protected override void apply()
    {
        // Inventory.Instance.AddKey(...) cuando exista el Inventory
        Debug.Log("Key collected!");
    }
}
