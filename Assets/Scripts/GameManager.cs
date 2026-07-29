using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private float time = 0f;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnCoinCollected += Lander_OnCoinCollected;
    }

    public int getScore()
    {
        return this.score;
    }

    public void addScore(int points)
    {
        this.score += points;
        Debug.Log("Score updated: " + this.score);
    }

    private void Lander_OnCoinCollected(object sender, Lander.CoinCollectedEventArgs coin)
    {
        this.addScore(coin.coinValue);
    }

    public void landed(float landingScore)
    {
        // Sumar al score el puntaje del landing exitoso
        this.addScore((int)(landingScore + this.time));
    }
    public void failLanding()
    {
        this.score = 0;
    }
    public float getTime()
    {
        return this.time;
    }
}
