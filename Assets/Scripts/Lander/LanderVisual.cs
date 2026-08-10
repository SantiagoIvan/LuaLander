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

    private void Lander_OnBeforeForceApplied(object sender, EventArgs e)
    {
        setThrusterParticleSystem(middleThrusterParticleSystem, false);
        setThrusterParticleSystem(leftThrusterParticleSystem, false);
        setThrusterParticleSystem(rightThrusterParticleSystem, false);
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
    private void Lander_OnFuelCollected(object sender, EventArgs e)
    {
        if (lander.getFuelAmount() > lander.getFuelThreshold())
        {
            this.deactivateSmokeParticleSystem();
        }
    }
}
