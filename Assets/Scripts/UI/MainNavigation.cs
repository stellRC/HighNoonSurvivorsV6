using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainNavigation : MonoBehaviour
{
    private FadingCanvas _fade;

    [Header("Canvas Groups")]
    [SerializeField]
    private GameObject _mainMenuCanvas;

    [SerializeField]
    private GameObject _gameMenuCanvas;

    [SerializeField]
    private GameObject _gameUICanvas;

    [SerializeField]
    private GameObject _loadingCanvas;

    [SerializeField]
    private GameObject _worldGameCanvas;

    [SerializeField]
    private GameObject _cooldownCanvas;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject _startMenu;

    [SerializeField]
    private GameObject _aboutMenu;

    [SerializeField]
    private GameObject _objectivesMenu;

    [SerializeField]
    private GameObject _settingsMenu;

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _gameOverMenu;

    [Header("Non-Menu Objects")]
    [SerializeField]
    private Slider _loadingSlider;

    [SerializeField]
    private GameObject _killCount;

    [SerializeField]
    private GameObject _settingsButton;

    public static bool IsPaused;

    private bool _state;

    private bool _isGameScene;

    private bool _loadObjectives;

    void Awake()
    {
        _state = false;
        _loadObjectives = false;
        _fade = gameObject.GetComponent<FadingCanvas>();
    }

    void Start()
    {
        InitializeObj_states();
    }

    void Update()
    {
        // // // Handle keyboard input
        if (Input.GetKeyDown(KeyCode.Escape) && _isGameScene && _gameOverMenu.activeSelf == false)
        {
            TogglePauseMenu();
        }
    }

    private void InitializeObj_states()
    {
        _mainMenuCanvas.SetActive(true);
        _gameMenuCanvas.SetActive(false);

        _startMenu.SetActive(true);
        _aboutMenu.SetActive(false);
        _objectivesMenu.SetActive(false);

        _loadingCanvas.SetActive(false);
        _gameUICanvas.SetActive(false);
        _settingsMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _gameOverMenu.SetActive(false);

        _cooldownCanvas.SetActive(false);
        _worldGameCanvas.SetActive(false);

        // Prevent pause menu from opening while in main menu
        IsPaused = true;

        // Start internal game clock (enable fog animations)
        Time.timeScale = 1f;

        // load objectives menu when navigating from game over scene for fast restart
        if (_loadObjectives)
        {
            ToggleObjectives();
            _loadObjectives = false;
        }
        _isGameScene = false;
    }

    public void SetMode(int mode)
    {
        if (mode == 0)
        {
            GameManager.Instance.IsEasyMode = true;
        }
        else
        {
            GameManager.Instance.IsEasyMode = false;
        }
        GameManager.Instance.LevelCycle();
    }

    public void LoadGame()
    {
        // hide menus
        _mainMenuCanvas.SetActive(false);

        _loadingCanvas.SetActive(true);
        _fade.FadeOut();

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
            _loadingSlider.value = progressValue;

            yield return null;
        }

        // Load main menu
        if (_isGameScene)
        {
            _fade.FadeIn();
            InitializeObj_states();
        }
        // Load game scene
        else
        {
            _fade.FadeIn();
            StartGame();
        }
    }

    public void ToggleAbout()
    {
        if (_startMenu.activeSelf)
        {
            _startMenu.SetActive(false);
            _aboutMenu.SetActive(true);
        }
        else
        {
            _startMenu.SetActive(true);
            _aboutMenu.SetActive(false);
        }
    }

    public void ToggleObjectives()
    {
        if (_startMenu.activeSelf)
        {
            _startMenu.SetActive(false);
            _fade.FadeIn();
            _objectivesMenu.SetActive(true);
        }
        else
        {
            _objectivesMenu.SetActive(false);

            _fade.FadeIn();
            _startMenu.SetActive(true);
        }
    }

    public void ReturnToObjectives()
    {
        GameManager.Instance.ResetValues();
        StartCoroutine(LoadLevelASync("MainMenu"));
        _loadObjectives = true;
    }

    // Load game scene
    public void StartGame()
    {
        GameManager.Instance.ResetValuesOnLoad();
        // Show kill count and settings button
        _gameUICanvas.SetActive(true);
        _settingsButton.SetActive(true);
        _killCount.SetActive(true);
        _worldGameCanvas.SetActive(true);

        _loadingCanvas.SetActive(false);
        _cooldownCanvas.SetActive(true);

        IsPaused = false;
        _isGameScene = true;
    }

    // Return to main menu from pause screen
    public void ReturnToMainMenu()
    {
        GameManager.Instance.ResetValues();
        StartCoroutine(LoadLevelASync("MainMenu"));
    }

    public void TogglePauseMenu()
    {
        if (!_isGameScene)
            return;

        if (!IsPaused)
        {
            // Stop in-game clock to stop animations and updates
            Time.timeScale = 0f;
            IsPaused = true;

            // clock.SetActive(false);
            _gameMenuCanvas.SetActive(true);
            _pauseMenu.SetActive(true);

            _gameUICanvas.SetActive(false);
            _settingsMenu.SetActive(false);
        }
        else if (IsPaused)
        {
            // Resume in game clock
            Time.timeScale = 1f;
            IsPaused = false;

            // clock.SetActive(true);
            _gameMenuCanvas.SetActive(false);
            _gameUICanvas.SetActive(true);
        }
    }

    public void ToggleGameOverMenu(float time)
    {
        _settingsButton.SetActive(false);
        _killCount.SetActive(false);

        // hide clock
        _worldGameCanvas.SetActive(false);

        _gameOverMenu.SetActive(true);
    }

    public void ToggleOptionsMenu()
    {
        if (_isGameScene)
        {
            _pauseMenu.SetActive(_state);
        }
        else
        {
            _gameMenuCanvas.SetActive(!_state);
            _mainMenuCanvas.SetActive(_state);
            _startMenu.SetActive(_state);
        }

        _settingsMenu.SetActive(!_state);

        _state = !_state;
    }

    // Exit application
    public void QuitGame()
    {
        Application.Quit();
    }
}
