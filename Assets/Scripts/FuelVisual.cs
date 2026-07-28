using UnityEngine;
using TMPro;
using System;

public class FuelVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro fuelAmountIndicator;
    [SerializeField] private Fuel fuel;

    private void Awake()
    {
        fuel = GetComponent<Fuel>();
        fuelAmountIndicator = GetComponentInChildren<TextMeshPro>();
        fuelAmountIndicator.text = "+" + fuel.getFuelAmount().ToString();

        fuel.OnFuelConsumed += Fuel_GetConsumed;
    }

    private void Fuel_GetConsumed(object sender, EventArgs e)
    {
        Debug.Log("Fuel consumption animation triggered.");
    }
}
