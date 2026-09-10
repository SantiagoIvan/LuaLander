using UnityEngine;

public class AsteroidVisual : MonoBehaviour
{
    [SerializeField] private Asteroid asteroid;
    [SerializeField] private GameObject explosionVFX;

    private void Awake()
    {
        asteroid.OnAsteroidCollision += Asteroid_OnAsteroidCollision;
    }
    private void Asteroid_OnAsteroidCollision(object sender, System.EventArgs e)
    {
        Instantiate(explosionVFX, asteroid.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
