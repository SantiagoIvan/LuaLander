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
        Lander.Instance.OnSuccessfulLanding += Lander_OnSuccessfulLanding;
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

    private void Lander_OnSuccessfulLanding(object sender, Lander.OnSuccessfulLandingEventArgs landing)
    {
        // Sumar al score el puntaje del landing exitoso
        this.addScore(landing.score);
    }
    public float getTime()
    {
        return this.time;
    }
}
