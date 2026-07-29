using UnityEngine;
using TMPro;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI landingSpeed;
    [SerializeField] private TextMeshProUGUI landingAngle;
    [SerializeField] private TextMeshProUGUI scoreMultiplier;
    [SerializeField] private TextMeshProUGUI timeLeft;
    [SerializeField] private TextMeshProUGUI finalScore;


    private void Start()
    {
        Lander.Instance.OnLanding += Lander_OnLanding;
        Debug.Log("LandedUI: Subscribed to landing event.");
        Debug.Log(Lander.Instance);
        gameObject.SetActive(false);
    }

    private void Lander_OnLanding(object sender, Lander.OnLandingEventArgs landing)
    {
        Debug.Log("LandedUI: Landing event received. " + landing.landingResult);
        // Actualizar la UI con los datos del aterrizaje exitoso
        if(landing.landingResult == Lander.LandingResult.Success)
        {
            title.text = "Landing Successful!";
        }
        else
        {
            title.text = "Landing Failed!";
            title.color = new Color(205f / 255f, 34f / 255f, 34f / 255f, 1f); // #CD2222 in Unity's Color format
        }
        landingSpeed.text = landing.landingSpeed.ToString("F2");
        landingAngle.text = landing.landingAngle.ToString("F2");
        scoreMultiplier.text = landing.multiplier.ToString();
        finalScore.text = landing.finalScore.ToString();
        gameObject.SetActive(true);
        Debug.Log("LandedUI: Game over");
    }
}
