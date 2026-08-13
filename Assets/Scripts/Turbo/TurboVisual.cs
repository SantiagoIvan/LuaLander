using UnityEngine;
using TMPro;
using System;

public class TurboVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro amountIndicator;
    [SerializeField] private Turbo turbo;
    [SerializeField] private ParticleSystem pickUpEffect;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        turbo = GetComponent<Turbo>();
        amountIndicator = GetComponentInChildren<TextMeshPro>();
        amountIndicator.text = "+" + turbo.getAmount().ToString();

        turbo.OnConsumed += Turbo_GetConsumed;
    }

    private void Turbo_GetConsumed(object sender, EventArgs e)
    {
        Debug.Log("Turbo consumption animation triggered. Falta animacion y sonidito.");
        if (pickUpEffect != null)
        {
            // Hacer la animacion de las particulas
        }
    }
}
