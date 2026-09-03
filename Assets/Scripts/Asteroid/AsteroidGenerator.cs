using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    private float timer;
    [SerializeField] private float lowBoundaryInterval = 1f;
    [SerializeField] private float highBoundaryInterval = 3f;
    [SerializeField] private Asteroid asteroidPrefab;

    private void Awake()
    {
        // Set the timer to a random value between 1 and 3 seconds
        SetRandomTimer();
    }

    // Update is called once per frame
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            // Obtengo el vector2 y lo paso a 3D para instanciar el asteroide en esa posición
            Vector2 spawnPosition = ScreenSpawnPointGenerator.Instance.GetRandomPointOutsideCamera();
            Vector3 finalSpawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
            Instantiate(asteroidPrefab, finalSpawnPosition, Quaternion.identity);
            Debug.Log("Asteroid generated in " + finalSpawnPosition);
            SetRandomTimer();
        }
    }

    private void SetRandomTimer()
    {
        timer = Random.Range(lowBoundaryInterval, highBoundaryInterval);
    }
    private void generateRandomSpawnPoint()
    {

    }
}
