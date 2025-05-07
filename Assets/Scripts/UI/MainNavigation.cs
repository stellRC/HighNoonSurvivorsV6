using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainNavigation : MonoBehaviour
{
    private FadingCanvas fade;

    [Header("Canvas Groups")]
    [SerializeField]
    private GameObject PersistentCanvas;

    [SerializeField]
    private GameObject gameUICanvas;

    [SerializeField]
    private GameObject loadingCanvas;

    [SerializeField]
    private GameObject NonPersistentCanvas;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject startMenu;

    [SerializeField]
    private GameObject aboutMenu;

    [SerializeField]
    private GameObject objectivesMenu;

    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private Slider loadingSlider;

    [SerializeField]
    private GameObject killCount;

    [SerializeField]
    private GameObject settingsButton;

    public static bool isPaused;

    private bool gameScene;

    private bool state;

    void Awake()
    {
        state = false;
        fade = gameObject.GetComponent<FadingCanvas>();
    }

    void Start()
    {
        InitializeObjStates();
    }

    void Update()
    {
        // // // Handle keyboard input
        if (Input.GetKeyDown(KeyCode.Escape) && gameScene && gameOverMenu.activeSelf == false)
        {
            TogglePauseMenu();
        }
    }

    private void InitializeObjStates()
    {
        NonPersistentCanvas.SetActive(true);
        PersistentCanvas.SetActive(false);

        startMenu.SetActive(true);
        aboutMenu.SetActive(false);
        objectivesMenu.SetActive(false);

        loadingCanvas.SetActive(false);
        gameUICanvas.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        // Prevent pause menu from opening while in main menu
        isPaused = true;
        gameScene = false;
        // Start internal game clock (enable fog animations)
        Time.timeScale = 1f;
    }

    public void LoadGame()
    {
        // hide menus
        NonPersistentCanvas.SetActive(false);

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
        if (gameScene)
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
    }

    // Load game scene
    public void StartGame()
    {
        // Show kill count and settings button
        gameUICanvas.SetActive(true);
        settingsButton.SetActive(true);
        killCount.SetActive(true);

        loadingCanvas.SetActive(false);

        isPaused = false;
        gameScene = true;
    }

    // Return to main menu from pause screen
    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadLevelASync("MainMenu"));
    }

    public void TogglePauseMenu()
    {
        if (!gameScene)
            return;

        if (!isPaused)
        {
            // Stop in-game clock to stop animations and updates
            Time.timeScale = 0f;
            isPaused = true;

            // clock.SetActive(false);
            PersistentCanvas.SetActive(true);
            pauseMenu.SetActive(true);

            gameUICanvas.SetActive(false);
            optionsMenu.SetActive(false);
        }
        else if (isPaused)
        {
            // Resume in game clock
            Time.timeScale = 1f;
            isPaused = false;

            // clock.SetActive(true);
            PersistentCanvas.SetActive(false);
            gameUICanvas.SetActive(true);
        }
    }

    public void ToggleGameOverMenu()
    {
        settingsButton.SetActive(false);
        killCount.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void ToggleOptionsMenu()
    {
        if (gameScene)
        {
            pauseMenu.SetActive(state);
        }
        else
        {
            PersistentCanvas.SetActive(!state);
            NonPersistentCanvas.SetActive(state);
            startMenu.SetActive(state);
        }

        optionsMenu.SetActive(!state);

        state = !state;
    }

    // Exit application
    public void QuitGame()
    {
        Application.Quit();
    }
}
