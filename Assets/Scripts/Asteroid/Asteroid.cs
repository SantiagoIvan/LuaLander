using UnityEngine;
using System;

public class Asteroid : MonoBehaviour
{
    private float speed = 1f;
    private float scale = 1f;
    private Vector3 direction = Vector3.zero;
    [SerializeField] private Rigidbody2D rigidbody2D;
    public event EventHandler OnAsteroidCollision;

    private void Awake()
    {
        speed = UnityEngine.Random.Range(0.5f, 10f);
        scale = UnityEngine.Random.Range(0.5f, 2f);
        transform.localScale = new Vector3(scale, scale, scale);
        // Rotate the asteroid towards ScreenSpawnPointGenerator.Instance.GetRandomPointAroundPlayer() with a random speed
        direction = (ScreenSpawnPointGenerator.Instance.GetRandomPointAroundPlayer() - (Vector2)transform.position).normalized * UnityEngine.Random.Range(0.5f, 2f);
        rigidbody2D.AddForce(direction * speed, ForceMode2D.Impulse);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnAsteroidCollision?.Invoke(this, EventArgs.Empty);
    }
}
