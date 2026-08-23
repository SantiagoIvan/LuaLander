using UnityEngine;

public abstract class PickUpReward : MonoBehaviour
{
    protected bool isCollected = false;
    [SerializeField] protected int amount = 1;
    protected abstract void apply();
    public bool wasCollected()
    {
        return isCollected;
    }
    public void getCollected()
    {
        this.isCollected = true;
        this.apply();
    }
    public int getAmount()
    {
        return amount;
    }
}
