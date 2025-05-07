using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private MainNavigation mainNavigation;

    [SerializeField]
    private GameOverManager gameOverManager;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private SkillTreeManager skillTreeManager;

    [SerializeField]
    private ObjectiveManager objectiveManager;

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
