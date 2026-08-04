using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        Time.timeScale = 1f;
        playButton.onClick.AddListener(() =>
        {
            GameManager.resetData();
            SceneLoader.LoadScene(SceneLoader.Scenes.GameScene);
        });
        settingsButton.onClick.AddListener(() =>
        {
            // Implement settings functionality here
            Debug.Log("Settings button clicked.");
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
            Debug.Log("Quit button clicked.");
        });
    }

    private void Start()
    {
        playButton.Select();
    }

}
