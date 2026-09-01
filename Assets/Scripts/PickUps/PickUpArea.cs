using UnityEngine;
using System;
public class PickUpArea : Pickupable
{
    // Renombrados: OnTriggerStay2D/OnTriggerExit2D son los nombres que Unity usa para
    // los mensajes magicos de trigger. Si un evento tuyo se llama igual, el dia que agregues
    // el metodo real de Unity para detectar la colision vas a tener un choque de nombres.
    public event EventHandler<OnAreaStayEventArgs> OnAreaStay;
    public event EventHandler OnAreaExit;

    [SerializeField] private float timeToPickup;
    private bool isCollected = false;
    private float timer = 0f;

    private protected void RaiseOnAreaStay()
    {
        float currentProgress = Mathf.Clamp01(timer / timeToPickup);
        this.OnAreaStay?.Invoke(this, new OnAreaStayEventArgs { progress = currentProgress});
    }
    private protected void RaiseOnAreaExit()
    {
        this.OnAreaExit?.Invoke(this, EventArgs.Empty);
    }
    private void reset()
    {
        this.timer = 0f;
    }
    public void onTriggerStay2D()
    {
        timer += Time.deltaTime;
        this.RaiseOnAreaStay();
        if (timer > timeToPickup && !isCollected)
        {
            isCollected = true;
            getPickedUp();
        }
    }
    public void onTriggerExit2D()
    {
        if (!isCollected)
        {
            this.RaiseOnAreaExit();
            this.reset();
        }
    }
}