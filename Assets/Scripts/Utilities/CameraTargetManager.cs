using UnityEngine;

public class CameraTargetManager : MonoBehaviour
{
    private float _gameDaySeconds;
    private float _currentTime;

    private bool _timerActive;

    private float _noonTime;

    void Start()
    {
        // Reset in Game manager prior to new round or if level data updates
        ResetTimer();
    }

    void Update()
    {
        // Increase time based on time clock and game day seconds
        // When game day seconds reaches 60, increment hours
        // Two circles, top one is transparent

        // Timer only increments while game is playing (and not while paused, on main menu, or after dying)
        if (_timerActive)
        {
            // in-game time is a fraction of real time. Ex: 1 real second is .5 in-game seconds
            _currentTime += Time.deltaTime * _gameDaySeconds;

            // Stop and reset timer when no longer playing
            if (!GameManager.Instance.MainNavigation.IsGameScene)
            {
                StopTimer();
                ResetTimer();
            }
            // Stop timer when player is dead, but don't reset time
            else if (GameManager.Instance.IsGameOver)
            {
                StopTimer();
            }
        }
    }

    public void StartTimer()
    {
        _timerActive = true;
    }

    public void StopTimer()
    {
        _timerActive = false;
    }

    public void ResetTimer()
    {
        _currentTime = 0;
        _noonTime = 12;

        // .5f in-game seconds for every 1 real time second
        _gameDaySeconds = GameManager.Instance.LevelData.GameDaySeconds;
    }
}
