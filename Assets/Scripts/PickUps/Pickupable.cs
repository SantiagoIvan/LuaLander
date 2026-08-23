using UnityEngine;
using System;

public class Pickupable : MonoBehaviour
{
    [SerializeField] protected PickUpReward reward;
    [SerializeField] protected float destroyDelay = 0;
    public event EventHandler OnPickedUp;

    public float getAmount() => this.reward.getAmount();
    public virtual void getPickedUp()
    {
        reward?.getCollected();
        RaiseOnPickedUp();
        destroySelf();
    }
    protected void destroySelf()
    {
        Destroy(gameObject, this.destroyDelay);
    }
    /// <summary>
    /// Los "event" en C# solo se pueden invocar (?.Invoke) desde la clase que los declara,
    /// ni siquiera las clases hijas pueden hacerlo directo. Por eso este metodo protected:
    /// las subclases lo llaman a el en vez de invocar OnPickedUp por su cuenta.
    /// </summary>
    protected void RaiseOnPickedUp()
    {
        OnPickedUp?.Invoke(this, EventArgs.Empty);
    }

}
