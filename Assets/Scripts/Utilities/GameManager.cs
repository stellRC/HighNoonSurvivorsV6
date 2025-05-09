using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private MainNavigation mainNavigation;

    private GameOverUIManager gameOverManager;

    private PlayerController playerController;

    private SkillTreeManager skillTreeManager;

    private ObjectivesManager objectiveManager;

    private PlayerCombat playerCombat;

    private ClockUI clockUI;

    private bool gameOverPanel;

    [Header("Game Stats")]
    public string timeCount;
    public int totalCount;

    public int brawlerCount;

    public int gunmanCount;

    public int projectileCount;

    public float hoursFloat;

    public bool isGameOver;

    public bool noDamage;

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
        playerController = FindFirstObjectByType<PlayerController>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        clockUI = GetComponentInChildren<ClockUI>();
        noDamage = false;
    }

    private void Start()
    {
        skillTreeManager.SetPlayerSkills(playerController.GetPlayerSkills());

        totalCount = 0;
        brawlerCount = 0;
        gunmanCount = 0;
        projectileCount = 0;
        hoursFloat = clockUI.hoursFloat;

        timeCount = clockUI.hoursString + ":" + clockUI.minutesString;
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

    // Update skills to unlock prior to next round
    private void UpdateObjectives()
    {
        if (brawlerCount >= 1)
        {
            objectiveManager.UpdateObjectiveValue("Slay 5 brawlers");
        }
        if (gunmanCount >= 1)
        {
            objectiveManager.UpdateObjectiveValue("Slay 10 gunmen");
        }
        if (projectileCount >= 1)
        {
            objectiveManager.UpdateObjectiveValue("Destroy 15 projectiles");
        }
        if (hoursFloat >= 12)
        {
            objectiveManager.UpdateObjectiveValue("Survive noon");
        }
    }
}
