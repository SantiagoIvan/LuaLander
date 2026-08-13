using UnityEngine;
using UnityEngine.UI;


public class TurboBarUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image turboBar;
    private Lander lander;
    private static readonly int IsTurboLowHash = Animator.StringToHash("IsLow");

    private void Start()
    {
        lander = Lander.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Turbo level: " + lander.getTurboAmount());
        // Actualizar el turbo
        turboBar.fillAmount = lander.getTurboAmount() / lander.getMaxTurboAmount();
        animator.SetBool(IsTurboLowHash, Lander.Instance.isTurboLow());

    }
}
