using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private MainNavigation mainNavigation;

    private GameOverUIManager gameOverManager;

    private PlayerController playerController;

    private SkillTreeManager skillTreeManager;

    private ObjectivesManager objectiveManager;

    public float timeCount;
    public int killCount;

    public bool isGameOver;

    private bool gameOverPanel;

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
    }

    private void Start()
    {
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
        }
        else
        {
            gameOverPanel = false;
        }
    }
}
