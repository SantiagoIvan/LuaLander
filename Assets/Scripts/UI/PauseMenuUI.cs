using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button soundVolumeButton;
    [SerializeField] private Button musicVolumeButton;
    [SerializeField] private RightClickHandler soundVolumeRightClick;
    [SerializeField] private RightClickHandler musicVolumeRightClick;

    private void Awake()
    {
        soundVolumeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.upSoundVolume();
            updateSoundVolume();
        });

        musicVolumeButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.upMusicVolume();
            updateMusicVolume();
        });
        soundVolumeRightClick.OnRightClick += () =>
        {
            SoundManager.Instance.downSoundVolume();
            updateSoundVolume();
        };
        musicVolumeRightClick.OnRightClick += () =>
        {
            MusicManager.Instance.downMusicVolume();
            updateMusicVolume();
        };

        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.resumeGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scenes.MainMenu);
        });
        updateMusicVolume();
        updateSoundVolume();

    }
    private void updateSoundVolume()
    {
        soundVolumeButton.GetComponentInChildren<TMPro.TMP_Text>().text = "SFX: " + SoundManager.Instance.getSoundVolume();
    }
    private void updateMusicVolume()
    {
        musicVolumeButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Music: " + MusicManager.Instance.getMusicVolume();
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        resumeButton.Select();
        this.Hide();
    }
    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        this.Show();
    }
    private void GameManager_OnGameResumed(object sender, System.EventArgs e)
    {
        this.Hide();
    }
    private void Show()
    {
        this.gameObject.SetActive(true);
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
