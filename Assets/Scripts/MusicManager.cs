using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private static float musicVolume = 5f;
    private static float maxMusicVolume = 10f;
    private static float timeMusic;


    private void Awake()
    {
        Instance = this;
        this.musicAudioSource.time = timeMusic;
    }

    private void Update()
    {
        timeMusic = this.musicAudioSource.time;
    }
    public float getMusicVolume()
    {
        return musicVolume;
    }
    public void setMusicVolume(float newMusicVolume)
    {
        musicVolume = newMusicVolume;
    }
    public float getMaxMusicVolume()
    {
        return maxMusicVolume;
    }
    public void upMusicVolume()
    {
        musicVolume = Mathf.Min(musicVolume + 1, maxMusicVolume);
        musicAudioSource.volume = getNormalizedMusicVolume();
        Debug.Log("Music volume increased to: " + musicVolume);
    }
    public void downMusicVolume()
    {
        musicVolume = Mathf.Max(musicVolume - 1, 0);
        musicAudioSource.volume = getNormalizedMusicVolume();
        Debug.Log("Music volume decreased to: " + musicVolume);
    }
    public float getNormalizedMusicVolume()
    {
        return musicVolume / maxMusicVolume;
    }
}
