using UnityEngine;
using TMPro;
using System;
public class CoinVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro indicatorTextMeshPro;
    [SerializeField] private GameObject indicatorGameObject;
    [SerializeField] private int INDICATOR_MIN_Z_ROTATION = -30;
    [SerializeField] private int INDICATOR_MAX_Z_ROTATION = 30;
    private Coin coin;

    private void Awake()
    {
        coin = GetComponent<Coin>();
        if (coin == null)
        {
            Debug.LogError("Coin component not found on the GameObject.");
            return;
        }
        // Subscribe to the OnPicked event
        indicatorTextMeshPro.text = $"+{coin.getValue()}";
        coin.OnPicked += Coin_OnPicked;
        this.HideText(); // Hide the indicator text initially
    }

    private void Coin_OnPicked(object sender, EventArgs e)
    {
        // Update the indicator text with the coin's value
        if (indicatorTextMeshPro != null)
        {
            
            int random = new System.Random().Next(this.INDICATOR_MIN_Z_ROTATION, this.INDICATOR_MAX_Z_ROTATION);
            indicatorGameObject.transform.rotation = Quaternion.Euler(0, 0, random);
            this.ShowText();
        }
    }
    private void HideText()
    {
        indicatorGameObject.SetActive(false);
    }
    private void ShowText()
    {
        indicatorGameObject.SetActive(true);
    }

}
