using Unity.Mathematics;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;

    [SerializeField]
    private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn audioSource
        AudioSource audioSource = Instantiate(
            soundFXObject,
            spawnTransform.position,
            quaternion.identity
        );

        // assign the audioClip
        audioSource.clip = audioClip;

        // assign volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        // assign random index
        int randomClip = UnityEngine.Random.Range(0, audioClip.Length);
        // Spawn audioSource
        AudioSource audioSource = Instantiate(
            soundFXObject,
            spawnTransform.position,
            quaternion.identity
        );

        // assign the audioClip
        audioSource.clip = audioClip[randomClip];

        // assign volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
