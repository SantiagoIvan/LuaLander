using UnityEngine;
using System;

public class Turbo : MonoBehaviour
{
    [SerializeField] private float amount = 10f; // Cantidad de combustible inicial
    public event EventHandler OnConsumed;

    public float getAmount()
    {
        return amount;
    }

    public void getConsumed()
    {
        OnConsumed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject); //Si lo destruyo aca, la animacion nunca sucede. Debo destruirlo al finalizar la animacion.
    }
}
