using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private MainNavigation mainNavigation;

    private GameOverUIManager gameOverManager;

    public PlayerController playerController;

    private SkillTreeManager skillTreeManager;

    private ObjectivesManager objectiveManager;

    private CooldownBar cooldownBar;

    public ClockUI clockUI;

    private bool gameOverPanel;

    [Header("Game Stats")]
    public int totalCount;

    public int brawlerCount;
    public int rollerCount;

    public int gunmanCount;

    public int projectileCount;

    public bool isGameOver;

    public bool noDamage;

    public bool canUseSpecial;

    public bool isPopping;

    public bool playerDead;

    [Header("Lvl Data")]
    public List<LevelData> LevelDataList = new();

    public int levelNumber;

    public LevelData levelData;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        else
        {
            Instance = this;
        }

        mainNavigation = GetComponentInChildren<MainNavigation>();
        skillTreeManager = GetComponentInChildren<SkillTreeManager>();
        objectiveManager = GetComponentInChildren<ObjectivesManager>();
        gameOverManager = GetComponentInChildren<GameOverUIManager>();
        cooldownBar = GetComponent<CooldownBar>();
        clockUI = GetComponentInChildren<ClockUI>();
        playerController = FindAnyObjectByType<PlayerController>();

        LevelCycle();

        // Value is updated when skills are unlocked and remains true when level ends
        canUseSpecial = false;
    }

    private void Start()
    {
        ResetValues();
        skillTreeManager.SetPlayerSkills(playerController.GetPlayerSkills());
    }

    void Update()
    {
        if (isGameOver && !gameOverPanel)
        {
            gameOverManager.OnGameOver();
            mainNavigation.ToggleGameOverMenu();

            gameOverPanel = true;
            isGameOver = false;

            UpdateObjectives();
        }
        else
        {
            gameOverPanel = false;
        }
    }

    private void LevelCycle()
    {
        levelData = LevelDataList[0];
    }

    // Reset reference values after new game scene loads
    public void ResetValuesOnLoad()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        noDamage = false;
        skillTreeManager.isSpecialAnim = false;
    }

    // Reset values when main menu is loaded during or after game via return to main menu button
    public void ResetValues()
    {
        totalCount = 0;
        brawlerCount = 0;
        gunmanCount = 0;
        projectileCount = 0;
        playerDead = false;
        noDamage = false;
        isGameOver = false;

        // Prevent player from dying before loading game if enemy collides with the player
        noDamage = true;

        // Reset cooldown bar to empty
        cooldownBar.ResetValues();
        // Destroy previous objectives
        objectiveManager.DestroyObjectives();
        // Instantiate updated objectives
        objectiveManager.CheckObjectiveValue();
    }

    // Check if skills are unlocked at end of level for use in next level
    private void UpdateObjectives()
    {
        Debug.Log(clockUI.hoursFloat);
        if (brawlerCount >= levelData.maxBrawlerCount)
        {
            objectiveManager.UpdateObjectiveValue(
                "Slay " + levelData.maxBrawlerCount + " brawlers"
            );
        }
        if (gunmanCount >= levelData.maxGunmanCount)
        {
            objectiveManager.UpdateObjectiveValue("Slay " + levelData.maxGunmanCount + " gunmen");
        }
        if (projectileCount >= levelData.maxProjectileCount)
        {
            objectiveManager.UpdateObjectiveValue(
                "Destroy " + levelData.maxProjectileCount + " projectiles"
            );
        }

        // The value of "noon" changes depending on clock settings
        if (clockUI.hoursFloat >= levelData.maxHourCount)
        {
            objectiveManager.UpdateObjectiveValue("survive past noon");
        }
    }
}
