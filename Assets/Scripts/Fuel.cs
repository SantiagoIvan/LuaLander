using UnityEngine;
using System;

public class Fuel : MonoBehaviour
{
    [SerializeField] private float fuelAmount = 50f; // Cantidad de combustible inicial
    public event EventHandler OnFuelConsumed;

    public float getFuelAmount()
    {
        return fuelAmount;
    }

    public void getConsumed()
    {
        OnFuelConsumed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject); //Si lo destruyo aca, la animacion nunca sucede. Debo destruirlo al finalizar la animacion.
    }
}
