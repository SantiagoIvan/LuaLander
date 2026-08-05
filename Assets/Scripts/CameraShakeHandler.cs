using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeHandler : MonoBehaviour
{
    [SerializeField] private float defaultForce = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Lander.Instance.OnLanding += Lander_OnLanding;
    }
    private void Lander_OnLanding(object sender, OnLandingEventArgs landingObj)
    {
        if (landingObj.landingResult == LandingResult.WrongLandingArea ||
            landingObj.landingResult == LandingResult.TooSteepLanding ||
            landingObj.landingResult == LandingResult.TooFastLanding ||
            landingObj.landingResult == LandingResult.TimeOut)
        {
            CinemachineImpulseSource impulseSource = GetComponent<CinemachineImpulseSource>();
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse(defaultForce);
            }
        }
    }
}
