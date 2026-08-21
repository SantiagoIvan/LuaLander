using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private float time;
    private State state = State.WaitingToStart;

    // El game manager tiene un nivel actual y tiene la lista de GameLevels posibles (los prefabs)
    // Entonces lo que hace es hacer el Instantiate para inicializar uno y mete el Lander en esa posicion.
    // Estatico para que persista entre escenas, sino, al actualizar el nivel y cargar nuevamente la escena, el objeto se destruye y se vuelve a crear con el default.
    private static int currentLevel = 1; 
    private static int finalScore = 0; 
    private static float startingFuelLimit = 100f;
    private static float startingTurboLimit = 40f;
    private static float startingGravity = 1f;
    private static int startingAccRate = 1000;
    private static float lowTimeThreshold = 5f;

    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCameraHandler cinemachineCameraHandler;
    [SerializeField] private int testLevel = 0;
    private static float startingTime = 30f;


    public event EventHandler OnTimeOut;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;
    public event EventHandler OnLowTime;
    private bool lowTimeTriggered = false;

    public static GameManager Instance { get; private set; }

    public static float getStartingTime()
    {
        return startingTime;
    }
    public static float getStartingTurboLimit() => startingTurboLimit;
    public static void setStartingTime(float newStartingTime)
    {
        startingTime = newStartingTime;
    }
    public static float getStartingFuelLimit() => startingFuelLimit;
    public static void setStartingFuelLimit(float newStartingFuelLimit)
    {
        startingFuelLimit = newStartingFuelLimit;
    }
    public static float getStartingGravity() => startingGravity;
    public static void setStartingGravity(float newStartingGravity)
    {
        startingGravity = newStartingGravity;
    }
    public static void setAccRate(int newAccRate)
    {
        startingAccRate = newAccRate;
    }
    public static int getAccRate() => startingAccRate;

    private void Awake()
    {
        Instance = this;
        if (this.testLevel > 0)
        {
            currentLevel = this.testLevel;
        }
        this.time = startingTime;
    }

    private void Start()
    {
        Lander.Instance.OnCoinCollected += Lander_OnCoinCollected;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;

        this.loadCurrentLevel();
    }

    public static void resetData()
    {
        currentLevel = 1;
        finalScore = 0;
    }
    private void Update()
    {
        if(this.time > 0 && this.state == State.Normal)
        {
            this.time -= Time.deltaTime;
            if (this.time < lowTimeThreshold && !lowTimeTriggered)
            {
                this.OnLowTime?.Invoke(this, EventArgs.Empty);
                lowTimeTriggered = true;
            }
        } else if (this.time <= 0 && this.state == State.Normal)
        {
            this.state = State.GameOver;
            OnTimeOut?.Invoke(this, EventArgs.Empty);
        }
    }

    public int getScore() => score;

    public void addScore(int points)
    {
        this.score += points;
        Debug.Log("Score updated: " + this.score);
    }

    private void Lander_OnCoinCollected(object sender, OnCoinCollectedEventArgs coin)
    {
        this.addScore(coin.coinValue);
        Debug.Log("score updated to: " + score);
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
        Time.timeScale = 0f;
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
        Debug.Log("Current state: " + this.state + " Loading level: " + currentLevel + " with final score " + finalScore);
        GameLevel gameLevel = this.getCurrentGameLevel();
        Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Transform landerPosition = gameLevel.getLanderStartPosition();
        Lander.Instance.transform.position = landerPosition.position;
        this.cinemachineCameraHandler.levelLoaded(gameLevel);
        Time.timeScale = 1f;
        this.lowTimeTriggered = false;
        
    }

    private GameLevel getCurrentGameLevel()
    {
        foreach (GameLevel gameLevel in this.gameLevelList)
        {
            if (gameLevel.getLevel() == currentLevel)
            {
                return gameLevel;
            }
        }
        return null;
    }
    public void nextLevel()
    {
        if (this.isLastLevel())
        {
            SceneLoader.LoadScene(SceneLoader.Scenes.GameOver);
            return;
        }
        currentLevel++;
        finalScore += this.score;
        SceneLoader.LoadScene(SceneLoader.Scenes.GameScene); // Esto va a cargar el GameScene, va a cargar el gameObject.
    }
    public void restartLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scenes.GameScene);
    }
    public static int getCurrentLevel()
    {
        return currentLevel;
    }
    public static int getFinalScore()
    {
        return finalScore;
    }
    public bool isLastLevel()
    {
        return currentLevel == this.gameLevelList.Count;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        if (this.state == State.Normal)
        {
            this.pauseGame();
        }
        else if (this.state == State.Paused)
        {
            this.resumeGame();
        }
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        this.state = State.Paused;
        this.OnGamePaused?.Invoke(this, EventArgs.Empty);
    }
    public void resumeGame()
    {
        Time.timeScale = 1f;
        this.state = State.Normal;
        this.OnGameResumed?.Invoke(this, EventArgs.Empty);
    }

    public void showFinalScore()
    {
        this.nextLevel();
    }
}
