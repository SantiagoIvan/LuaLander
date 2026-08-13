using UnityEngine;
using UnityEngine.UI;


public class FuelBarUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image fuelBar;
    private Lander lander;
    private static readonly int IsFuelLowHash = Animator.StringToHash("IsLow");

    private void Start()
    {
        lander = Lander.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizar el fuel
        fuelBar.fillAmount = lander.getFuelAmount() / lander.getMaxFuelAmount();
        animator.SetBool(IsFuelLowHash, Lander.Instance.isFuelLow());

    }
}
