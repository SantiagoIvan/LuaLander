using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI finalScoreText;


    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scenes.MainMenu);
        });
    }

    private void Start()
    {
        this.finalScoreText.text = "Final Score: " + GameManager.getFinalScore().ToString();
    }
}
