using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI landingSpeed;
    [SerializeField] private TextMeshProUGUI landingAngle;
    [SerializeField] private TextMeshProUGUI scoreMultiplier;
    [SerializeField] private TextMeshProUGUI timeLeft;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI nextButtonText;
    [SerializeField] private Transform centerPosition;
    [SerializeField] private Button showFinalScoreButton;

    private Action nextButtonClickAction;
    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() => {
            SceneLoader.LoadScene(SceneLoader.Scenes.MainMenu);
        } );
        nextButton.onClick.AddListener(() => {
            this.nextButtonClickAction();
        } );
        showFinalScoreButton.onClick.AddListener(() =>
        {
            GameManager.Instance.showFinalScore();
        });
        this.showFinalScoreButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        Lander.Instance.OnLanding += Lander_OnLanding;
        Debug.Log("LandedUI: Subscribed to landing event.");
        Debug.Log(Lander.Instance);
        nextButton.Select();
        gameObject.SetActive(false);
    }

    private void Lander_OnLanding(object sender, OnLandingEventArgs landing)
    {
        Debug.Log("LandedUI: Landing event received. " + landing.landingResult);
        // Actualizar la UI con los datos del aterrizaje exitoso
        if(landing.landingResult == LandingResult.Success)
        {
            title.text = "Landing Successful!";
            title.color = Color.white; // White color for success
            if (GameManager.Instance.isLastLevel())
            {
                nextButton.gameObject.SetActive(false);
                mainMenuButton.gameObject.SetActive(false);
                showFinalScoreButton.gameObject.transform.position = centerPosition.position;
                showFinalScoreButton.Select();
                showFinalScoreButton.gameObject.SetActive(true);
            }
            else
            {
                nextButtonText.text = "Next level";
                nextButtonClickAction = GameManager.Instance.nextLevel;
            }
        }
        else
        {
            title.text = "Landing Failed!";
            title.color = new Color(205f / 255f, 34f / 255f, 34f / 255f, 1f); // #CD2222 in Unity's Color format
            nextButtonText.text = "Restart";
            nextButtonClickAction = GameManager.Instance.restartLevel;
        }
        landingSpeed.text = landing.landingSpeed.ToString("F2");
        landingAngle.text = landing.landingAngle.ToString("F2");
        scoreMultiplier.text = landing.multiplier.ToString();
        finalScore.text = landing.finalScore.ToString();
        gameObject.SetActive(true);
        Debug.Log("LandedUI: Game over");
    }
}
