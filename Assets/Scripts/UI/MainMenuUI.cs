using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1); // Load the game scene (assuming it's at index 1)
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


}
