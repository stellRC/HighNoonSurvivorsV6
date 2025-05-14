using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDoDamage
{
    [Header("Data")]
    public PlayerData PlayerData;
    private MasterAnimator _playerAnimator;

    private PlayerSkills _playerSkills;
    private PlayerInput _playerInput;

    private float _playerHealth;

    private void Awake()
    {
        // instance of player skills
        _playerSkills = new PlayerSkills();

        _playerAnimator = GetComponent<MasterAnimator>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.enabled = true;
    }

    // Set up health incase future development will have more than one hit death
    private void OnEnable()
    {
        _playerHealth = PlayerData.MaxHealth;
    }

    public PlayerSkills GetPlayerSkills()
    {
        return _playerSkills;
    }

    // Damage from enemy or projectile collision
    public void DoDamage(int damage)
    {
        _playerHealth--;

        if (_playerHealth <= 0)
        {
            DeathAnimation();
            DeathAudio();
            DisablePlayer();
        }
    }

    private void DeathAudio()
    {
        SoundEffectsManager.instance.PlayRandomSoundFXClip(
            SoundEffectsManager.instance.deathSoundClips,
            transform,
            .20f
        );
    }

    // Prevent further player animation and enemy death
    private void DisablePlayer()
    {
        _playerInput.enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;
        this.enabled = false;
    }

    // Death animation (not looped)
    private void DeathAnimation()
    {
        _playerAnimator.ChangeAnimation(_playerAnimator.stateAnimation[0]);
    }
}
