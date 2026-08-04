using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private TMP_InputField gravity;
    [SerializeField] private TMP_InputField fuelLimit;
    [SerializeField] private TMP_InputField time;
    [SerializeField] private TMP_InputField accRate;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        gravity.text = GameManager.getStartingGravity().ToString();
        fuelLimit.text = GameManager.getStartingFuelLimit().ToString();
        time.text = GameManager.getStartingTime().ToString();
        accRate.text = GameManager.getAccRate().ToString();

        saveButton.onClick.AddListener(() =>
        {
            GameManager.setStartingGravity(float.Parse(gravity.text));
            GameManager.setStartingFuelLimit(float.Parse(fuelLimit.text));
            GameManager.setStartingTime(float.Parse(time.text));
            GameManager.setAccRate(int.Parse(accRate.text));
            this.goBack();
            Debug.Log("Settings saved.");
        });
        backButton.onClick.AddListener(() =>
        {
            this.goBack();
        });
        this.gameObject.SetActive(false);
    }

    private void goBack()
    {
        this.mainMenuUI.showMainMenuOptions();
    }

    public void Show()
    {
        this.loadValues();
        this.gameObject.SetActive(true);
    }

    private void loadValues()
    {
        gravity.text = GameManager.getStartingGravity().ToString();
        fuelLimit.text = GameManager.getStartingFuelLimit().ToString();
        time.text = GameManager.getStartingTime().ToString();
        accRate.text = GameManager.getAccRate().ToString();
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
