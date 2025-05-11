using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDoDamage
{
    private MasterAnimator playerAnimator;

    private PlayerSkills playerSkills;
    private PlayerInput playerInput;

    private void Awake()
    {
        // instance of player skills
        playerSkills = new PlayerSkills();

        playerAnimator = GetComponent<MasterAnimator>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
    }

    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }

    // Damage from enemy or projectile collision
    public void DoDamage(int damage)
    {
        DeathAnimation();
        DeathAudio();
        DisablePlayer();
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
        playerInput.enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;
        this.enabled = false;
    }

    // Death animation (not looped)
    private void DeathAnimation()
    {
        playerAnimator.ChangeAnimation(playerAnimator.stateAnimation[0]);
    }
}
