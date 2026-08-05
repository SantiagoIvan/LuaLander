using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;
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
}
