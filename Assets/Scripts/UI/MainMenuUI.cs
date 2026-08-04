using UnityEngine;
using UnityEngine.UI;
using System;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject logoGameObject;
    [SerializeField] private GameObject settingsUIGameObject;
    [SerializeField] private GameObject mainMenuOptionsGameObject;


    private void Awake()
    {
        Time.timeScale = 1f;
        settingsUIGameObject.SetActive(false);
        playButton.onClick.AddListener(() =>
        {
            GameManager.resetData();
            SceneLoader.LoadScene(SceneLoader.Scenes.GameScene);
        });
        settingsButton.onClick.AddListener(() =>
        {
            // Implement settings functionality here
            Debug.Log("Settings button clicked.");
            this.showSettingsUI();
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
    public void showSettingsUI()
    {
        logoGameObject.SetActive(false);
        mainMenuOptionsGameObject.SetActive(false);
        settingsUIGameObject.SetActive(true);
    }

    public void showMainMenuOptions()
    {
        logoGameObject.SetActive(true);
        mainMenuOptionsGameObject.SetActive(true);
        settingsUIGameObject.SetActive(false);
    }
}
