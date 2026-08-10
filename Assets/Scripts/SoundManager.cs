using UnityEngine;
using System;
public class SoundManager : MonoBehaviour
{
    // Singleton pattern
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip fuelPickupSound;
    [SerializeField] private AudioClip lowFuelWarning;
    [SerializeField] private AudioClip outOfFuel;
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip successfulLanding;
    [SerializeField] private static float soundVolume = 5f;
    private static float maxSoundVolume = 10f;

    public event EventHandler OnSoundVolumeChanged;

    [SerializeField] private AudioSource currentWarning;
    
    private void Awake()
    {
        Instance = this;
        Debug.Log("SoundManager Awake: soundVolume = " + soundVolume);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Lander.Instance.OnFuelCollected += Lander_OnFuelCollected;
        Lander.Instance.OnCoinCollected += Lander_OnCoinCollected;
        Lander.Instance.OnLanding += Lander_OnLanding;
        Lander.Instance.OnLowFuel += Lander_OnLowFuel;
        Lander.Instance.OnOutOfFuel += Lander_OnOutOfFuel;
    }
    public void setSoundVolume(int newSoundVolume)
    {
        soundVolume = newSoundVolume;
    }
    public float getSoundVolume()
    {
        return soundVolume;
    }
    public float getMaxSoundVolume()
    {
        return maxSoundVolume;
    }
    private void Lander_OnFuelCollected(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupSound, Lander.Instance.transform.position, this.getNormalizedSoundVolume()); // Spawnea un sound object que le digas para reproducir el sonido.
        if (Lander.Instance.getFuelAmount() > Lander.Instance.getFuelThreshold())
        {
            // Stop LowFUelWarning or OutOfFuel AudioSource
            this.currentWarning.Stop();
        }
    }
    private void Lander_OnLowFuel(object sender, EventArgs e)
    {
        // Play LowFuelWarning AudioSource in loop
        this.playWarning(this.lowFuelWarning);
    }
    private void Lander_OnOutOfFuel(object sender, EventArgs e)
    {
        // Play OutOfFuel AudioClip in loop and stop LowFuelWarning AudioSource
        this.playWarning(this.outOfFuel);
    }
    private void Lander_OnCoinCollected(object sender, OnCoinCollectedEventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupSound, Lander.Instance.transform.position, this.getNormalizedSoundVolume());
    }
    private void Lander_OnLanding(object sender, OnLandingEventArgs e)
    {
        this.currentWarning.Stop();
        if(e.landingResult == LandingResult.Success)
        {
            AudioSource.PlayClipAtPoint(successfulLanding, Lander.Instance.transform.position, this.getNormalizedSoundVolume());
        }
        else
        {
            AudioSource.PlayClipAtPoint(crashSound, Lander.Instance.transform.position, this.getNormalizedSoundVolume());
        }
    }
    public void upSoundVolume()
    {
        soundVolume = Mathf.Min(soundVolume + 1, maxSoundVolume);
        Debug.Log("Sound volume increased to: " + soundVolume);
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }
    public void downSoundVolume()
    {
        soundVolume = Mathf.Max(soundVolume - 1, 0);
        Debug.Log("Sound volume decreased to: " + soundVolume);
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }
    public float getNormalizedSoundVolume()
    {
        return soundVolume / maxSoundVolume;
    }
    private void playWarning(AudioClip src)
    {
        
        if(this.currentWarning != null && this.currentWarning.clip == src && this.currentWarning.isPlaying) 
            return;

        this.currentWarning.clip = src;
        this.currentWarning.loop = true;
        this.currentWarning.volume = this.getNormalizedSoundVolume();
        this.currentWarning.Play();
    }
}
