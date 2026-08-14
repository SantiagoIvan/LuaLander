using UnityEngine;
using System;
public abstract class PickupableVisual : MonoBehaviour
{
    [SerializeField] protected Pickupable pickupable;
    [SerializeField] protected ParticleSystem pickUpEffect; // para largar el efecto de particulas

    protected virtual void Awake()
    {
        pickupable.OnPickedUp += Pickupable_OnPickedUp;
    }
    protected abstract void Pickupable_OnPickedUp(object sender, EventArgs e);
}
