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
        level.text = GameManager.getCurrentLevel().ToString();
        lander.OnLowFuel += Lander_OnLowFuel;
        lander.OnLowTurbo += Lander_OnLowTurbo;
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
        fuelBar.fillAmount = lander.getFuelAmount() / lander.getMaxFuelAmount();

        // Actualizar el tiempo
        time.text = GameManager.Instance.getTime().ToString("F0");

        // Actualiza el turbo
        // turboBar

        animator.SetBool(IsFuelLowHash, lander.isFuelLow());
        // falta el animator del turbo
    }
    private void Lander_OnLowFuel(object sender, EventArgs e)
    {
        animator.SetBool(IsFuelLowHash, lander.isFuelLow());
    }
    private void GameManager_OnLowTime(object sender, EventArgs e)
    {
        Debug.Log("Gametime is low");
        animator.SetBool(IsTimeLowHash, true); // TODO Corregir animator porque solo puede estar este o el fuelBar animado. Tal vez hay que separar en animators
    }
    private void Lander_OnLowTurbo(object sender, EventArgs e)
    {
        Debug.Log("Turbo level: " + lander.getTurboAmount());
    }
}
