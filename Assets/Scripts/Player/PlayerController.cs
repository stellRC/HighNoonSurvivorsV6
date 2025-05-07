using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDoDamage
{
    private MasterAnimator playerAnimator;

    private PlayerSkills playerSkills;

    private void Awake()
    {
        // instance of player skills
        playerSkills = new PlayerSkills();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        playerAnimator = GetComponent<MasterAnimator>();
    }

    private void PlayerSkills_OnSkillUnlocked(
        object sender,
        PlayerSkills.OnSkillUnlockedEventArgs e
    )
    {
        switch (e.skillType)
        {
            case PlayerSkills.SkillType.SpeedBoost:
                SetMovementSpeed(5f);
                break;
        }
    }

    // Increase movement speed with upgrade
    private void SetMovementSpeed(float speed)
    {
        throw new NotImplementedException();
    }

    // Check if EarthShatter skill is unlocked
    public bool CanUseEarthshatter()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.Earthshatter);
    }

    public bool CanUseElectrocute()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.Electrocute);
    }

    public bool CanUseSpeedBoost()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.SpeedBoost);
    }

    public bool CanUseThrowOverarm()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.ThrowOverarm);
    }

    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }

    // Damage from enemy or projectile collision
    public void DoDamage(int damage)
    {
        DeathAnimation();
        DisablePlayer();
    }

    // Prevent further player animation and enemy death
    private void DisablePlayer()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;
    }

    // Death animation (not looped)
    private void DeathAnimation()
    {
        playerAnimator.ChangeAnimation(playerAnimator.stateAnimation[0]);
    }
}
