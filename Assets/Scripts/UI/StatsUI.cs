using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    // El UGUI es para UI Objects como el CANVAS
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI speedX;
    [SerializeField] private TextMeshProUGUI speedY;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private Image fuelBar;
    private float maxFuelAmount;

    // Para las flechitas
    [SerializeField] private GameObject upArrowGameObject;
    [SerializeField] private GameObject leftArrowGameObject;
    [SerializeField] private GameObject rightArrowGameObject;
    [SerializeField] private GameObject downArrowGameObject;

    private void Start()
    {
        this.maxFuelAmount = Lander.Instance.getMaxFuelAmount();
        level.text = GameManager.getCurrentLevel().ToString();
    }

    private void Update()
    {
        // Escondo las flechas y activo solamente las que correspondan.
        upArrowGameObject.SetActive(false);
        rightArrowGameObject.SetActive(false);
        leftArrowGameObject.SetActive(false);
        downArrowGameObject.SetActive(false);

        // Actualizar el score
        score.text = GameManager.Instance.getScore().ToString();
        
        // Actualizar la velocidad y las flechas
        float xspeed = Lander.Instance.getSpeedX();
        float yspeed = Lander.Instance.getSpeedY();

        upArrowGameObject.SetActive(yspeed >= 0f);
        rightArrowGameObject.SetActive(xspeed >= 0f);
        leftArrowGameObject.SetActive(xspeed < 0f);
        downArrowGameObject.SetActive(yspeed < 0f);
        speedX.text = xspeed.ToString("F2");
        speedY.text = yspeed.ToString("F2");
        
        // Actualizar el fuel
        fuelBar.fillAmount = Lander.Instance.getFuelAmount() / this.maxFuelAmount;
        
        // Actualizar el tiempo
        time.text = GameManager.Instance.getTime().ToString("F0");
    }
}
