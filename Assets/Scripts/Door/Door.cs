using UnityEngine;
using System;
public class Door : MonoBehaviour
{
    [SerializeField] private Pickupable pickUpAreaOpenDoor;
    [SerializeField] private ItemType requiredKey;
    private void Awake()
    {
        if (pickUpAreaOpenDoor != null)
        {
            pickUpAreaOpenDoor.OnPickedUp += PickUpAreaOpenDoor_OnPickedUp;
        }
    }

    private void PickUpAreaOpenDoor_OnPickedUp(object sender, EventArgs e)
    {
        TryGetComponent<DoorVisual>(out var doorVisual);
        doorVisual?.openDoor();
        
    }
}
