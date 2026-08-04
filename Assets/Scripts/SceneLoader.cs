using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scenes
    {
        MainMenu = 0,
        GameScene = 1,
        GameOver = 2,
    }
    public static void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}
