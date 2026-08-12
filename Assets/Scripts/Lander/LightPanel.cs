using UnityEngine;

public class LightPanel : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] Animator animator;
    private int lightUpTrigger = Animator.StringToHash("LightUp");
    private static float minInterval = 0.5f;
    private static float maxInterval = 4.5f;

    private void Awake()
    {
        // Setear random Timer
        this.computeRandomTimer();
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0f)
        {
            animator.SetTrigger(lightUpTrigger);
            this.computeRandomTimer();
            Debug.Log("Animation Panel triggered. Next in " + timer.ToString());
        }
    }
    private void computeRandomTimer()
    {
        this.timer = UnityEngine.Random.Range(minInterval, maxInterval);
    }
}
