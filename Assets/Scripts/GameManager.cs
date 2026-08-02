using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private float time = 30f;
    private State state = State.WaitingToStart;

    // El game manager tiene un nivel actual y tiene la lista de GameLevels posibles (los prefabs)
    // Entonces lo que hace es hacer el Instantiate para inicializar uno y mete el Lander en esa posicion.
    private static int currentLevel = 1; // Estatico para que persista entre escenas, sino, al actualizar el nivel y cargar nuevamente la escena, el objeto se destruye y se vuelve a crear con el default.
    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCameraHandler cinemachineCameraHandler;
    [SerializeField] private int testLevel = 0;

    public event EventHandler OnTimeOut;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        if (this.testLevel > 0)
        {
            currentLevel = this.testLevel;
        }
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
        } else if (this.time <= 0 && this.state == State.Normal)
        {
            this.state = State.GameOver;
            OnTimeOut?.Invoke(this, EventArgs.Empty);
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
        this.state = state.newState;
        if(this.state == State.Normal)
        {
            this.cinemachineCameraHandler.levelStarted();
        }
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
        Debug.Log("Current level is: " + currentLevel.ToString());
        foreach(GameLevel gameLevel in this.gameLevelList)
        {
            if(gameLevel.getLevel() == currentLevel)
            {
                GameLevel newGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                Transform landerPosition = newGameLevel.getLanderStartPosition();
                Lander.Instance.transform.position = landerPosition.position;
                this.cinemachineCameraHandler.levelLoaded(newGameLevel);
            }
        }
    }


    public void nextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene(0); // Esto va a cargar el GameScene, va a cargar el gameObject.
    }
    public void restartLevel()
    {
        SceneManager.LoadScene(0);
    }
    public static int getCurrentLevel()
    {
        return currentLevel;
    }
    public bool isLastLevel()
    {
        return currentLevel == this.gameLevelList.Count;
    }
}
