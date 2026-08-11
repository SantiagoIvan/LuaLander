using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;
    [SerializeField] private AudioClip OutOfFuelAudioClip;
    [SerializeField] private AudioClip lowFuelAudioClip;
    [SerializeField] private AudioSource warningAudioSource;
    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();
    }
    private void Start()
    {
        this.thrusterAudioSource.Pause();
        lander.OnBeforeForceApplied += Lander_OnBeforeForceApplied;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnOutOfFuel += Lander_OnOutOfFuel;
        lander.OnLowFuel += Lander_OnLowFuel;
        lander.OnFuelCollected += Lander_OnFuelCollected;
        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChanged;
    }
    private void SoundManager_OnSoundVolumeChanged(object sender, System.EventArgs e)
    {
        this.thrusterAudioSource.volume = SoundManager.Instance.getNormalizedSoundVolume();
    }
    private void Lander_OnBeforeForceApplied(object sender, System.EventArgs e)
    {
        this.thrusterAudioSource.Pause();
    }
    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        if (!this.thrusterAudioSource.isPlaying)
        {
            this.thrusterAudioSource.Play();
        }
    }
    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        if (!this.thrusterAudioSource.isPlaying)
        {
            this.thrusterAudioSource.Play();
        }
    }
    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        if (!this.thrusterAudioSource.isPlaying)
        {
            this.thrusterAudioSource.Play();
        }
    }
    private void Lander_OnOutOfFuel(object sender, System.EventArgs e)
    {
        this.playWarning(this.OutOfFuelAudioClip);
    }
    private void Lander_OnLowFuel(object sender, System.EventArgs e)
    {
        this.playWarning(this.lowFuelAudioClip);
    }
    private void playWarning(AudioClip src)
    {
        if (this.warningAudioSource != null && this.warningAudioSource.clip == src && this.warningAudioSource.isPlaying && lander.getState() != State.GameOver)
            return;
        this.warningAudioSource.clip = src;
        this.warningAudioSource.loop = true;
        this.warningAudioSource.volume = SoundManager.Instance.getNormalizedSoundVolume();
        this.warningAudioSource.Play();
    }
    private void Lander_OnFuelCollected(object sender, OnFuelCollectedEventArgs e)
    {
        if (!Lander.Instance.isFuelLow())
        {
            // Stop LowFUelWarning or OutOfFuel AudioSource
            this.warningAudioSource.Stop();
        }
    }
}
