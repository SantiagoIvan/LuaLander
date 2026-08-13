using System;
using UnityEngine;

/// <summary>
/// Encapsula la logica comun a un recurso consumible con umbral de "bajo" y
/// evento de "vacio" (patron identico al que tenian Fuel y Turbo duplicado en Lander).
/// No es un MonoBehaviour: es una clase de datos comun, se crea con "new" desde
/// quien la use (en este caso, Lander).
/// </summary>
public class ResourceMeter
{
    public event EventHandler OnLow;
    public event EventHandler OnOutOf;

    private float amount;
    private float maxAmount;
    private readonly float threshold;
    private float consumptionRate;

    public ResourceMeter(float initialAmount, float maxAmount, float threshold, float consumptionRate)
    {
        this.maxAmount = maxAmount;
        this.amount = Mathf.Min(initialAmount, maxAmount);
        this.threshold = threshold;
        this.consumptionRate = consumptionRate;
    }

    public float GetAmount() => amount;
    public float GetMaxAmount() => maxAmount;
    public float GetThreshold() => threshold;
    public bool IsLow() => amount < threshold;

    /// <summary>
    /// Resta consumptionRate * deltaTime y dispara OnLow / OnOutOf segun corresponda.
    /// Mantiene el mismo comportamiento que el codigo original: no clampea a 0,
    /// el amount puede quedar levemente negativo.
    /// </summary>
    public void Consume(float deltaTime)
    {
        if (amount > 0)
        {
            amount -= consumptionRate * deltaTime;
        }

        if (amount <= 0f)
        {
            OnOutOf?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (amount < threshold)
        {
            OnLow?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>Suma el valor recolectado, sin superar el maximo.</summary>
    public void Add(float value)
    {
        amount = Mathf.Min(maxAmount, amount + value);
    }

    public void SetMaxAmount(float newMax)
    {
        maxAmount = newMax;
        amount = Mathf.Min(amount, maxAmount);
    }
}