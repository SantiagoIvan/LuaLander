using UnityEngine;
using TMPro;
using System;

public class TurboVisual : PickupableVisual
{
    [SerializeField] private TextMeshPro amountIndicator;
    [SerializeField] private Animator animator;

    override protected void Awake()
    {
        base.Awake();
        amountIndicator = GetComponentInChildren<TextMeshPro>();
        amountIndicator.text = "+" + pickupable.getAmount().ToString();

    }

    override protected void Pickupable_OnPickedUp(object sender, EventArgs e)
    {
        Debug.Log("Turbo picked up!.");
    }
}
