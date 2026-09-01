using UnityEngine;

public class Key : PickUpReward
{
    [SerializeField] private ItemType type;

    protected override void apply()
    {
        InventoryManager.Instance.AddItem(type);
    }
}
