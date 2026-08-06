using UnityEngine;
using System;
public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 25; // Valor de la moneda
    private bool isCollected = false; // Bandera para verificar si la moneda ha sido recogida
    public event EventHandler OnPicked;

    public int getValue()
    {
        return value;
    }

    public void getCollected()
    {
        if (isCollected)
        {
            return;
        }
        this.isCollected = true;
        // disparar animacion de particulas y sonido
        OnPicked?.Invoke(this, EventArgs.Empty);
        //get destroyed after a delay
        Destroy(gameObject, 0.5f);
    }
    
}
