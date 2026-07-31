using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private float time = 30f;
    private State state = State.WaitingToStart;

    // El game manager tiene un nivel actual y tiene la lista de GameLevels posibles (los prefabs)
    // Entonces lo que hace es hacer el Instantiate para inicializar uno y mete el Lander en esa posicion.
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private List<GameLevel> gameLevelList;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnCoinCollected += Lander_OnCoinCollected;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        this.loadCurrentLevel();
    }

    private void Update()
    {
        if(this.time > 0 && this.state == State.Normal)
        {
            this.time -= Time.deltaTime;
        }
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

    private void Lander_OnCoinCollected(object sender, OnCoinCollectedEventArgs coin)
    {
        this.addScore(coin.coinValue);
    }

    private void Lander_OnStateChanged(object sender, OnStateChangedEventArgs state)
    {
        this.state = State.Normal;
    }

    public void landed(float landingScore)
    {
        // Sumar al score el puntaje del landing exitoso
        this.addScore((int)(landingScore + this.time));
    }
    public void failLanding()
    {
        this.score = 0;
        this.state = State.GameOver;
    }
    public float getTime()
    {
        return this.time;
    }

    private void loadCurrentLevel()
    {
        foreach(GameLevel gameLevel in this.gameLevelList)
        {
            if(gameLevel.getLevel() == this.currentLevel)
            {
                GameLevel newGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                Transform landerPosition = newGameLevel.getLanderStartPosition();
                Lander.Instance.transform.position = landerPosition.position;
            }
        }
    }


    public void nextLevel()
    {
        this.currentLevel++;
        SceneManager.LoadScene(0);
    }
    public void restartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
