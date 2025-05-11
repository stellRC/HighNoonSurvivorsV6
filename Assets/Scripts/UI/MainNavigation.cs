using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainNavigation : MonoBehaviour
{
    private FadingCanvas fade;

    [Header("Canvas Groups")]
    [SerializeField]
    private GameObject mainMenuCanvas;

    [SerializeField]
    private GameObject gameMenuCanvas;

    [SerializeField]
    private GameObject gameUICanvas;

    [SerializeField]
    private GameObject loadingCanvas;

    [SerializeField]
    private GameObject worldGameCanvas;

    [SerializeField]
    private GameObject cooldownCanvas;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject startMenu;

    [SerializeField]
    private GameObject aboutMenu;

    [SerializeField]
    private GameObject objectivesMenu;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject gameWinMenu;

    [Header("Non-Menu Objects")]
    [SerializeField]
    private Slider loadingSlider;

    [SerializeField]
    private GameObject killCount;

    [SerializeField]
    private GameObject settingsButton;

    [SerializeField]
    private ObjectivesManager objectivesManager;

    public static bool isPaused;

    private bool state;

    private bool isGameScene;

    private bool loadObjectives;

    void Awake()
    {
        state = false;
        loadObjectives = false;
        fade = gameObject.GetComponent<FadingCanvas>();
    }

    void Start()
    {
        InitializeObjStates();
    }

    void Update()
    {
        // // // Handle keyboard input
        if (Input.GetKeyDown(KeyCode.Escape) && isGameScene && gameOverMenu.activeSelf == false)
        {
            TogglePauseMenu();
        }
    }

    private void InitializeObjStates()
    {
        mainMenuCanvas.SetActive(true);
        gameMenuCanvas.SetActive(false);

        startMenu.SetActive(true);
        aboutMenu.SetActive(false);
        objectivesMenu.SetActive(false);

        loadingCanvas.SetActive(false);
        gameUICanvas.SetActive(false);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gameWinMenu.SetActive(false);
        cooldownCanvas.SetActive(false);
        worldGameCanvas.SetActive(false);

        // Prevent pause menu from opening while in main menu
        isPaused = true;

        // Start internal game clock (enable fog animations)
        Time.timeScale = 1f;

        // load objectives menu when navigating from game over scene for fast restart
        if (loadObjectives)
        {
            ToggleObjectives();
            loadObjectives = false;
        }
        isGameScene = false;
    }

    public void SetMode(int mode)
    {
        if (mode == 0)
        {
            GameManager.Instance.isEasyMode = true;
        }
        else
        {
            GameManager.Instance.isEasyMode = false;
        }
        GameManager.Instance.LevelCycle();
    }

    public void LoadGame()
    {
        // hide menus
        mainMenuCanvas.SetActive(false);

        loadingCanvas.SetActive(true);
        fade.FadeOut();

        // Run Async
        StartCoroutine(LoadLevelASync("Level_One"));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        // Fill progress bar while scene is loading
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;

            yield return null;
        }

        // Load main menu
        if (isGameScene)
        {
            fade.FadeIn();
            InitializeObjStates();
        }
        // Load game scene
        else
        {
            fade.FadeIn();
            StartGame();
        }
    }

    public void ToggleAbout()
    {
        if (startMenu.activeSelf)
        {
            startMenu.SetActive(false);
            aboutMenu.SetActive(true);
        }
        else
        {
            startMenu.SetActive(true);
            aboutMenu.SetActive(false);
        }
    }

    public void ToggleObjectives()
    {
        if (startMenu.activeSelf)
        {
            startMenu.SetActive(false);
            fade.FadeIn();
            objectivesMenu.SetActive(true);
        }
        else
        {
            objectivesMenu.SetActive(false);

            fade.FadeIn();
            startMenu.SetActive(true);
        }
    }

    public void ReturnToObjectives()
    {
        GameManager.Instance.ResetValues();
        StartCoroutine(LoadLevelASync("MainMenu"));
        loadObjectives = true;
    }

    // Load game scene
    public void StartGame()
    {
        GameManager.Instance.ResetValuesOnLoad();
        // Show kill count and settings button
        gameUICanvas.SetActive(true);
        settingsButton.SetActive(true);
        killCount.SetActive(true);
        worldGameCanvas.SetActive(true);

        loadingCanvas.SetActive(false);
        cooldownCanvas.SetActive(true);

        isPaused = false;
        isGameScene = true;
    }

    // Return to main menu from pause screen
    public void ReturnToMainMenu()
    {
        GameManager.Instance.ResetValues();
        StartCoroutine(LoadLevelASync("MainMenu"));
    }

    public void TogglePauseMenu()
    {
        if (!isGameScene)
            return;

        if (!isPaused)
        {
            // Stop in-game clock to stop animations and updates
            Time.timeScale = 0f;
            isPaused = true;

            // clock.SetActive(false);
            gameMenuCanvas.SetActive(true);
            pauseMenu.SetActive(true);

            gameUICanvas.SetActive(false);
            settingsMenu.SetActive(false);
        }
        else if (isPaused)
        {
            // Resume in game clock
            Time.timeScale = 1f;
            isPaused = false;

            // clock.SetActive(true);
            gameMenuCanvas.SetActive(false);
            gameUICanvas.SetActive(true);
        }
    }

    public void ToggleGameOverMenu(float time)
    {
        settingsButton.SetActive(false);
        killCount.SetActive(false);
        // if (time >= 12)
        // {
        //     gameWinMenu.SetActive(true);
        // }
        // else { }
        gameOverMenu.SetActive(true);
        // hide clock
        worldGameCanvas.SetActive(false);
    }

    public void ToggleOptionsMenu()
    {
        if (isGameScene)
        {
            pauseMenu.SetActive(state);
        }
        else
        {
            gameMenuCanvas.SetActive(!state);
            mainMenuCanvas.SetActive(state);
            startMenu.SetActive(state);
        }

        settingsMenu.SetActive(!state);

        state = !state;
    }

    // Exit application
    public void QuitGame()
    {
        Application.Quit();
    }
}
