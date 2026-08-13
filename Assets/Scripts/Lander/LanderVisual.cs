using UnityEngine;
using System;

/*
 * Esta clase escucha por lo eventos emitidos por el Lander script.
 * El lander script, cuando detecta que se presiono una tecla de movimiento, emite un evento que es escuchado por esta clase.
 * Al detectar ese evento, enciende la emision de particulas del thruster correspondiente.
 */
public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;

    [SerializeField] private ParticleSystem leftSmokeParticleSystem;
    [SerializeField] private ParticleSystem middleSmokeParticleSystem;
    [SerializeField] private ParticleSystem rightSmokeParticleSystem;

    [SerializeField] private GameObject explosionVFX;

    private readonly static string BASE_COLOR = "#FFFFFF";
    private readonly static string TURBO_COLOR = "#003AFF";

    // Colores ya parseados una sola vez en Awake, para no parsear el string cada vez que se usan
    private Color baseColor;
    private Color turboColor;

    private Lander lander; // tambien se puede hacer serializeField


    private void Awake()
    {
        lander = GetComponent<Lander>();

        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForceApplied += Lander_OnBeforeForceApplied;
        lander.OnLanding += Lander_OnLanding;
        lander.OnOutOfFuel += Lander_OnOutOfFuel;
        lander.OnFuelCollected += Lander_OnFuelCollected;
        lander.OnTurboPressed += Lander_OnTurboPressed;

        if (!ColorUtility.TryParseHtmlString(BASE_COLOR, out baseColor))
        {
            Debug.LogWarning($"No se pudo parsear BASE_COLOR ({BASE_COLOR}), usando blanco por defecto.");
            baseColor = Color.white;
        }
        if (!ColorUtility.TryParseHtmlString(TURBO_COLOR, out turboColor))
        {
            Debug.LogWarning($"No se pudo parsear TURBO_COLOR ({TURBO_COLOR}), usando rojo por defecto.");
            turboColor = Color.red;
        }

        setThrusterParticleSystem(leftThrusterParticleSystem, false);
        setThrusterParticleSystem(middleThrusterParticleSystem, false);
        setThrusterParticleSystem(rightThrusterParticleSystem, false);
        setThrusterParticleSystem(leftSmokeParticleSystem, false);
        setThrusterParticleSystem(middleSmokeParticleSystem, false);
        setThrusterParticleSystem(rightSmokeParticleSystem, false);
    }

    private void Lander_OnLanding(object sender, OnLandingEventArgs landingObj)
    {
        switch (landingObj.landingResult)
        {
            case LandingResult.WrongLandingArea:
            case LandingResult.TooSteepLanding:
            case LandingResult.TooFastLanding:
            case LandingResult.TimeOut:
                Instantiate(this.explosionVFX, transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);
                break;
        }

    }
    private void setThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = enabled;

    }

    /// <summary>
    /// Setea el StartColor de un ParticleSystem. Hay que guardar el "main" en una
    /// variable local antes de modificarlo: es un struct devuelto por valor, no se
    /// puede encadenar la asignacion directo sobre particleSystem.main.startColor.
    /// </summary>
    private void setThrusterStartColor(ParticleSystem particleSystem, Color color)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color);
    }

    private void Lander_OnBeforeForceApplied(object sender, EventArgs e)
    {
        // Apaga las particulas de fuel/turbo. Las de humito salen siempre cuando estas out orf fuel
        setThrusterParticleSystem(middleThrusterParticleSystem, false);
        setThrusterParticleSystem(leftThrusterParticleSystem, false);
        setThrusterParticleSystem(rightThrusterParticleSystem, false);

        setThrusterStartColor(leftThrusterParticleSystem, baseColor);
        setThrusterStartColor(rightThrusterParticleSystem, baseColor);
        setThrusterStartColor(middleThrusterParticleSystem, baseColor);
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        setThrusterParticleSystem(middleThrusterParticleSystem, true);
        setThrusterParticleSystem(leftThrusterParticleSystem, true);
        setThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        setThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        setThrusterParticleSystem(leftThrusterParticleSystem, true);
    }
    private void Lander_OnTurboPressed(object sender, EventArgs e)
    {
        setThrusterStartColor(leftThrusterParticleSystem, turboColor);
        setThrusterStartColor(rightThrusterParticleSystem, turboColor);
        setThrusterStartColor(middleThrusterParticleSystem, turboColor);
    }
    private void Lander_OnOutOfFuel(object sender, EventArgs e)
    {
        activateSmokeParticleSystem();
    }
    private void activateSmokeParticleSystem()
    {
        setThrusterParticleSystem(leftSmokeParticleSystem, true);
        setThrusterParticleSystem(rightSmokeParticleSystem, true);
        setThrusterParticleSystem(middleSmokeParticleSystem, true);
    }
    private void deactivateSmokeParticleSystem()
    {
        setThrusterParticleSystem(leftSmokeParticleSystem, false);
        setThrusterParticleSystem(rightSmokeParticleSystem, false);
        setThrusterParticleSystem(middleSmokeParticleSystem, false);
    }
    private void Lander_OnFuelCollected(object sender, OnFuelCollectedEventArgs e)
    {
        if (lander.getFuelAmount() > lander.getFuelThreshold())
        {
            this.deactivateSmokeParticleSystem();
        }
    }
}