using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private MainNavigation _mainNavigation;

    private GameOverUIManager _gameOverManager;

    public PlayerController PlayerController;

    private SkillTreeManager _skillTreeManager;

    private ObjectivesManager _objectiveManager;

    private CooldownBar _cooldownBar;

    public ClockUI ClockUI;

    private bool _gameOverPanel;

    [Header("Game Stats")]
    public int TotalCount;

    public int BrawlerCount;
    public int RollerCount;

    public int GunmanCount;

    public int ProjectileCount;

    public bool IsGameOver;

    public bool NoDamage;

    public bool CanUseSpecial;

    public bool PlayerDead;

    public bool IsEasyMode;

    [Header("Lvl Data")]
    public List<LevelData> LevelDataList = new();

    public int LevelNumber;

    public LevelData LevelData;

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

        _mainNavigation = GetComponentInChildren<MainNavigation>();
        _skillTreeManager = GetComponentInChildren<SkillTreeManager>();
        _objectiveManager = GetComponentInChildren<ObjectivesManager>();
        _gameOverManager = GetComponentInChildren<GameOverUIManager>();
        _cooldownBar = GetComponent<CooldownBar>();
        ClockUI = GetComponentInChildren<ClockUI>();
        PlayerController = FindAnyObjectByType<PlayerController>();

        // Value is updated when skills are unlocked and remains true when level ends
        CanUseSpecial = false;
        IsEasyMode = true;
        LevelData = LevelDataList[0];
    }

    private void Start()
    {
        ResetValues();
        _skillTreeManager.SetPlayerSkills(PlayerController.GetPlayerSkills());
    }

    void Update()
    {
        if (IsGameOver && !_gameOverPanel)
        {
            _gameOverManager.OnGameOver();
            _mainNavigation.ToggleGameOverMenu(ClockUI.HoursFloat);

            _gameOverPanel = true;
            IsGameOver = false;

            UpdateObjectives();
        }
        else
        {
            _gameOverPanel = false;
        }
    }

    public void LevelCycle()
    {
        if (IsEasyMode)
        {
            LevelData = LevelDataList[0];
        }
        else
        {
            LevelData = LevelDataList[1];
        }
        _objectiveManager.SwitchObjectives();
    }

    // Reset reference values after new game scene loads
    public void ResetValuesOnLoad()
    {
        PlayerController = FindAnyObjectByType<PlayerController>();
        NoDamage = false;
        _skillTreeManager.isSpecialAnim = false;
    }

    // Reset values when main menu is loaded during or after game via return to main menu button
    public void ResetValues()
    {
        TotalCount = 0;
        BrawlerCount = 0;
        GunmanCount = 0;
        ProjectileCount = 0;
        PlayerDead = false;
        NoDamage = false;
        IsGameOver = false;

        // Prevent player from dying before loading game if enemy collides with the player
        NoDamage = true;

        // Reset cooldown bar to empty
        _cooldownBar.ResetValues();
        // Destroy previous objectives
        _objectiveManager.DestroyObjectives();
        // Instantiate updated objectives
        _objectiveManager.CheckObjectiveValue();
    }

    // Check if skills are unlocked at end of level for use in next level
    private void UpdateObjectives()
    {
        if (BrawlerCount >= LevelData.MaxBrawlerCount)
        {
            _objectiveManager.UpdateObjectiveValue(
                "Slay " + LevelData.MaxBrawlerCount + " brawlers"
            );
        }
        if (GunmanCount >= LevelData.MaxGunmanCount)
        {
            _objectiveManager.UpdateObjectiveValue("Slay " + LevelData.MaxGunmanCount + " gunmen");
        }
        if (ProjectileCount >= LevelData.MaxRollerCount)
        {
            _objectiveManager.UpdateObjectiveValue("Slay " + LevelData.MaxRollerCount + " rollers");
        }

        // The value of "noon" changes depending on clock settings
        if (ClockUI.HoursFloat >= LevelData.MaxHourCount)
        {
            _objectiveManager.UpdateObjectiveValue("survive past noon");
        }
    }
}
