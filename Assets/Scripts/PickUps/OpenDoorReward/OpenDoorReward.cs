using UnityEngine;

public class OpenDoorReward : PickUpReward
{
    [SerializeField] private ItemType requiredKey;
    override protected void apply()
    {
        Debug.Log("Key used!");
        InventoryManager.Instance.UseItem(requiredKey);
    }
}
