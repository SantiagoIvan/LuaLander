using UnityEngine;
using UnityEngine.UI;
using System;
public class PickUpAreaVisual : PickupableVisual
{
    private PickUpArea pickUpArea;
    [SerializeField] private Image progressImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Awake()
    {
        base.Awake();
        this.pickUpArea = GetComponent<PickUpArea>();
        pickUpArea.OnAreaStay += PickUpArea_OnAreaStay;
        pickUpArea.OnAreaExit += PickUpArea_OnAreaExit;
    }
    override protected void Pickupable_OnPickedUp(object sender, EventArgs e)
    {
        Debug.Log("Picked up! Instanciar particulas");
    }
    private void PickUpArea_OnAreaStay(object sender, OnAreaStayEventArgs e)
    {
        Debug.Log("Filling circle to " + e.progress);
        progressImage.fillAmount = e.progress;
    }
    private void PickUpArea_OnAreaExit(object sender, EventArgs e)
    {
        Debug.Log("Reseting circle");
        progressImage.fillAmount = 0;
    }
}
