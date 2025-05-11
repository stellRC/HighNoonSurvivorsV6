using System.Collections;
using System.Diagnostics;
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

    [Header("Skill Buttons")]
    [SerializeField]
    private Button earthBtn;

    [SerializeField]
    private Button electroBtn;

    [SerializeField]
    private Button spinBtn;

    [SerializeField]
    private Button swordBtn;

    [SerializeField]
    private Color unlockedColor;

    [SerializeField]
    private Color lockedColor;

    [SerializeField]
    private TMP_Text skillText;

    private ObjectivesManager objectiveManager;

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
            playerAnimator = FindAnyObjectByType<PlayerMovement>().playerAnimator;
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
            UnlockUI(spinBtn, 0);

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
                "Slay " + GameManager.Instance.levelData.maxBrawlerCount + " brawlers"
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
                "Slay " + GameManager.Instance.levelData.maxBrawlerCount + " brawlers to unlock!";
        }
    }

    private void UnlockSkillElectro()
    {
        if (
            objectiveManager.skillObjectives[
                "Slay " + GameManager.Instance.levelData.maxGunmanCount + " gunmen"
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
                "Slay " + GameManager.Instance.levelData.maxGunmanCount + " gunmen to unlock!";
        }
    }

    // Can only destroy projectiles with either fog or lightning skills
    private void UnlockSkillEarth()
    {
        if (
            objectiveManager.skillObjectives[
                "Destroy " + GameManager.Instance.levelData.maxProjectileCount + " projectiles"
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
                "Destroy "
                + GameManager.Instance.levelData.maxProjectileCount
                + " projectiles to unlock!";
        }
    }

    private void UnlockUI(Button button, int specialMove)
    {
        TransformReset();
        TransformScale(button);
        chosenSpecialMove = specialMove;
        playerAnimator.ChangeAnimation(playerAnimator.specialAnimation[chosenSpecialMove]);
        isSpecialAnim = true;

        button.GetComponentInChildren<TMP_Text>().text = " ";
        if (!GameManager.Instance.canUseSpecial)
        {
            GameManager.Instance.canUseSpecial = true;
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
                    swordBtn.GetComponent<Image>().color = unlockedColor;
                    break;
                case PlayerSkills.SkillType.Spin:
                    spinBtn.GetComponent<Image>().color = unlockedColor;
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
