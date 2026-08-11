using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class StatsUI : MonoBehaviour
{
    // El UGUI es para UI Objects como el CANVAS
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI speedX;
    [SerializeField] private TextMeshProUGUI speedY;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private Image fuelBar;
    [SerializeField] private LowFuelUI lowFuelUI;
    private float maxFuelAmount;
    private Lander lander;

    // Para las flechitas
    [SerializeField] private GameObject upArrowGameObject;
    [SerializeField] private GameObject leftArrowGameObject;
    [SerializeField] private GameObject rightArrowGameObject;
    [SerializeField] private GameObject downArrowGameObject;
    [SerializeField] private Animator animator;
    private static readonly int IsFuelLowHash = Animator.StringToHash("IsFuelLow");
    private static readonly int IsTimeLowHash = Animator.StringToHash("IsTimeLow");

    private void Start()
    {
        lander = Lander.Instance;
        this.maxFuelAmount = lander.getMaxFuelAmount();
        level.text = GameManager.getCurrentLevel().ToString();
        lander.OnLowFuel += Lander_OnLowFuel;
        GameManager.Instance.OnLowTime += GameManager_OnLowTime;
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
        fuelBar.fillAmount = lander.getFuelAmount() / this.maxFuelAmount;
        
        // Actualizar el tiempo
        time.text = GameManager.Instance.getTime().ToString("F0");

        animator.SetBool(IsFuelLowHash, lander.isFuelLow());
        
    }
    private void Lander_OnLowFuel(object sender, EventArgs e)
    {
        animator.SetBool(IsFuelLowHash, lander.isFuelLow());
    }
    private void GameManager_OnLowTime(object sender, EventArgs e)
    {
        animator.SetBool(IsTimeLowHash, true);
    }
}
