using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    // Complete an objective to gain a new skill on the skill wheel
    // Face the final boss when all skills are gained

    // Press skill button to unlock a skill that is ted to a key press

    private PlayerSkills playerSkills;

    [Header("Skill Buttons")]
    [SerializeField]
    private Button earthBtn;

    [SerializeField]
    private Button electroBtn;

    [SerializeField]
    private Button speedBtn;

    [SerializeField]
    private Button throwBtn;

    private Color unlockedColor;

    [SerializeField]
    private Color lockedColor;

    [SerializeField]
    private ObjectivesManager objectiveManager;

    public int chosenSpecialMove;

    private void Awake()
    {
        earthBtn.onClick.AddListener(UnlockSkillEarth);
        electroBtn.onClick.AddListener(UnlockSkillElectro);
        speedBtn.onClick.AddListener(UnlockSkillSpeed);
        throwBtn.onClick.AddListener(UnlockSkillThrow);

        unlockedColor = Color.white;
        chosenSpecialMove = 0;
    }

    private void UnlockSkillSpeed()
    {
        if (objectiveManager.skillObjectives["Kill 10 Zombies"])
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.SpeedBoost);
            chosenSpecialMove = 1;
        }
    }

    private void UnlockSkillElectro()
    {
        if (objectiveManager.skillObjectives["Kill 20 Zombies"])
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.Electrocute);
            chosenSpecialMove = 2;
        }
    }

    private void UnlockSkillEarth()
    {
        if (objectiveManager.skillObjectives["Kill 30 Zombies"])
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.Earthshatter);
            chosenSpecialMove = 3;
        }
    }

    private void UnlockSkillThrow()
    {
        if (objectiveManager.skillObjectives["Kill 40 Zombies"])
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.ThrowOverarm);
            chosenSpecialMove = 4;
        }
    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
    }

    private void PlayerSkills_OnSkillUnlocked(
        object sender,
        PlayerSkills.OnSkillUnlockedEventArgs e
    )
    {
        UpdateVisuals(e.skillType);
    }

    private void UpdateVisuals(PlayerSkills.SkillType skillType)
    {
        if (playerSkills.CanUnlockSkill(skillType))
        {
            switch (skillType)
            {
                case PlayerSkills.SkillType.Electrocute:
                    electroBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.Earthshatter:
                    earthBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.ThrowOverarm:
                    throwBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.SpeedBoost:
                    speedBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.None:
                    break;
            }
        }
    }
}
