using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    // Complete an objective to gain a new skill on the skill wheel
    // Face the final boss when all skills are gained

    // Press skill button to unlock a skill that is ted to a key press

    private PlayerSkills playerSkills;

    [SerializeField]
    private MasterAnimator playerAnimator;
    private ObjectivesManager objectiveManager;

    [Header("Skill Buttons")]
    [SerializeField]
    private Button earthBtn;

    [SerializeField]
    private Button electroBtn;

    [SerializeField]
    private Button spinBtn;

    [SerializeField]
    private Button swordBtn;

    [Header("Color & Text")]
    [SerializeField]
    private Color unlockedColor;

    [SerializeField]
    private Color lockedColor;

    [SerializeField]
    private TMP_Text skillText;

    public int chosenSpecialMove;

    public bool isSpecialAnim;

    private void Awake()
    {
        objectiveManager = GetComponent<ObjectivesManager>();

        earthBtn.onClick.AddListener(UnlockSkillEarth);
        electroBtn.onClick.AddListener(UnlockSkillElectro);
        spinBtn.onClick.AddListener(UnlockSkillSpin);
        swordBtn.onClick.AddListener(UnlockSwordCombo);

        chosenSpecialMove = -1;
        isSpecialAnim = false;
    }

    private void Start()
    {
        SetColors();
    }

    private void Update()
    {
        if (playerAnimator == null)
        {
            playerAnimator = FindAnyObjectByType<PlayerMovement>().PlayerAnimator;
        }
    }

    private void SetColors()
    {
        electroBtn.GetComponent<Image>().color = lockedColor;

        earthBtn.GetComponent<Image>().color = lockedColor;

        swordBtn.GetComponent<Image>().color = lockedColor;

        spinBtn.GetComponent<Image>().color = lockedColor;
    }

    private void UnlockSkillSpin()
    {
        if (objectiveManager.skillObjectives["survive past noon"])
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.Spin);

            skillText.text = "Unlocked: Invincibility";
        }
        else
        {
            skillText.text = "survive past noon to unlock!";
        }
    }

    private void UnlockSwordCombo()
    {
        if (
            objectiveManager.skillObjectives[
                "Slay " + GameManager.Instance.LevelData.MaxBrawlerCount + " brawlers"
            ]
        )
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.SwordCombo);
            UnlockUI(swordBtn, 1);
            skillText.text = "Unlocked: Increased Attack Range";
        }
        else
        {
            skillText.text =
                "Slay " + GameManager.Instance.LevelData.MaxBrawlerCount + " brawlers to unlock!";
        }
    }

    private void UnlockSkillElectro()
    {
        if (
            objectiveManager.skillObjectives[
                "Slay " + GameManager.Instance.LevelData.MaxGunmanCount + " gunmen"
            ]
        )
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.ShockHeavy);
            chosenSpecialMove = 2;
            UnlockUI(electroBtn, 2);
            skillText.text = "Unlocked: Lightning Bolt";
        }
        else
        {
            skillText.text =
                "Slay " + GameManager.Instance.LevelData.MaxGunmanCount + " gunmen to unlock!";
        }
    }

    private void UnlockSkillEarth()
    {
        if (
            objectiveManager.skillObjectives[
                "Slay " + GameManager.Instance.LevelData.MaxRollerCount + " rollers"
            ]
        )
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.GroundSlam);
            UnlockUI(earthBtn, 3);
            skillText.text = "Unlocked: Death Fog";
        }
        else
        {
            skillText.text =
                "Slay " + GameManager.Instance.LevelData.MaxRollerCount + " rollers to unlock!";
        }
    }

    private void UnlockUI(Button button, int specialMove)
    {
        TransformReset();
        TransformScale(button);
        chosenSpecialMove = specialMove;
        playerAnimator.ChangeAnimation(playerAnimator.SpecialAnimation[chosenSpecialMove]);
        isSpecialAnim = true;

        button.GetComponentInChildren<TMP_Text>().text = " ";
        if (!GameManager.Instance.CanUseSpecial)
        {
            GameManager.Instance.CanUseSpecial = true;
        }
    }

    // Set player skills when unlocked
    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
    }

    // Update unlocked elements to reflect unlock
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
                    swordBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.Spin:
                    spinBtn.GetComponent<Image>().color = unlockedColor;
                    UnlockUI(spinBtn, 0);
                    break;
                case PlayerSkills.SkillType.None:
                    break;
            }
        }
    }

    // Rebind animations to fix animations not playing on enable


    private void TransformScale(Button button)
    {
        button.transform.DOScale(1.1f, 0.25f).SetUpdate(true);
    }

    private void TransformReset()
    {
        spinBtn.transform.DOScale(1f, 0.15f).SetUpdate(true);
        electroBtn.transform.DOScale(1f, 0.15f).SetUpdate(true);
        spinBtn.transform.DOScale(1f, 0.15f).SetUpdate(true);
        swordBtn.transform.DOScale(1f, 0.15f).SetUpdate(true);
    }
}
