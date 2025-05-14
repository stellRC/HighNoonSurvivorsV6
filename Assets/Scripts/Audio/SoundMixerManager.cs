using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;

    // Interpolate logarithmic value of volume to linear so volume change sounds more natural
    public void SetMasterVolume(float volumeLevel)
    {
        _audioMixer.SetFloat("masterVolume", Mathf.Log10(volumeLevel) * 20f);
    }

    public void SetSoundFXVolume(float volumeLevel)
    {
        _audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volumeLevel) * 20f);
    }

    public void SetMusicVolume(float volumeLevel)
    {
        _audioMixer.SetFloat("musicVolume", Mathf.Log10(volumeLevel) * 20f);
    }
}
