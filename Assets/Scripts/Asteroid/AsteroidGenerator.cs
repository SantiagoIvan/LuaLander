using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    private float timer;
    [SerializeField] private float lowBoundaryInterval = 1f;
    [SerializeField] private float highBoundaryInterval = 3f;

    private void Awake()
    {
        // Set the timer to a random value between 1 and 3 seconds
        SetRandomTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            Debug.Log("Asteroid generated");
            SetRandomTimer();
        }
    }

    private void SetRandomTimer()
    {
        timer = Random.Range(lowBoundaryInterval, highBoundaryInterval);
    }
}
