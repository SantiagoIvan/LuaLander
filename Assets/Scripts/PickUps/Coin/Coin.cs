using UnityEngine;
using System;
public class Coin : Pickupable
{
    private bool isCollected = false; // Bandera para verificar si la moneda ha sido recogida
    
    private void Awake()
    {
        destroyDelay = 0.5f;
    }

    override public void getPickedUp()
    {
        if (isCollected)
        {
            return;
        }
        this.isCollected = true;
        // disparar animacion de particulas y sonido
        base.getPickedUp();
    }
    
}
