using UnityEngine;
using System;

// Para las monedas
public class OnCoinCollectedEventArgs : EventArgs
{
    public int coinValue;
    public OnCoinCollectedEventArgs(int value)
    {
        coinValue = value;
    }
}
