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
    private Button spinBtn;

    [SerializeField]
    private Button gunBtn;

    private Color unlockedColor;

    [SerializeField]
    private Color lockedColor;

    private ObjectivesManager objectiveManager;

    public int chosenSpecialMove;

    private void Awake()
    {
        objectiveManager = GetComponent<ObjectivesManager>();

        earthBtn.onClick.AddListener(UnlockSkillEarth);
        electroBtn.onClick.AddListener(UnlockSkillElectro);
        spinBtn.onClick.AddListener(UnlockSkillSpin);
        gunBtn.onClick.AddListener(UnlockSkillGun);

        unlockedColor = Color.white;
        chosenSpecialMove = -1;
    }

    private void UnlockSkillSpin()
    {
        if (objectiveManager.skillObjectives["Stay alive past noon"])
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.Spin);
            chosenSpecialMove = 0;
        }
    }

    private void UnlockSkillElectro()
    {
        if (objectiveManager.skillObjectives["Slay 5 brawlers"])
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.SwordCombo);
            chosenSpecialMove = 1;
        }
    }

    private void UnlockSkillEarth()
    {
        if (objectiveManager.skillObjectives["Slay 10 gunmen"])
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.ShockHeavy);
            chosenSpecialMove = 2;
        }
    }

    // Can only destroy projectiles with either fog or lightning skills
    private void UnlockSkillGun()
    {
        if (objectiveManager.skillObjectives["Destroy 15 projectiles"])
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.GroundSlam);
            chosenSpecialMove = 3;
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
                case PlayerSkills.SkillType.ShockHeavy:
                    electroBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.GroundSlam:
                    earthBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.SwordCombo:
                    gunBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.Spin:
                    spinBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.None:
                    break;
            }
        }
    }
}
