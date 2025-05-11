using Unity.Mathematics;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;

    [SerializeField]
    private GameObject soundFXObject;

    [Header("Player Audio")]
    public AudioClip[] normalPlayerAttackSingleSoundClips;

    public AudioClip[] deathSoundClips;

    public AudioClip[] playerRunningClips;

    public AudioClip[] playerDashingClips;
    public AudioClip[] playerIdleClips;

    [Header("Enemy Audio")]
    public AudioClip[] rollingSoundClips;

    public AudioClip[] shootingSoundClips;

    public AudioClip[] particleDeathSoundClips;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //  Pool audioSource

        GameObject audioSource = ObjectPooling.SpawnObject(
            soundFXObject,
            spawnTransform.position,
            quaternion.identity,
            ObjectPooling.PoolType.Audio
        );
        AudioSource source = audioSource.GetComponent<AudioSource>();
        // assign the audioClip
        source.clip = audioClip;

        // assign volume
        source.volume = volume;

        // Play sound
        source.Play();
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        // assign random index
        int randomClip = UnityEngine.Random.Range(0, audioClip.Length);

        if (audioClip[randomClip] != null)
        {
            // Spawn audioSource

            GameObject audioSource = ObjectPooling.SpawnObject(
                soundFXObject,
                spawnTransform.position,
                quaternion.identity,
                ObjectPooling.PoolType.Audio
            );
            AudioSource source = audioSource.GetComponent<AudioSource>();

            // assign the audioClip
            source.clip = audioClip[randomClip];

            // assign volume
            source.volume = volume;

            // Play sound
            source.Play();
        }
    }
}
