using System;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    // // an in-game day is 6 minutes long
    // private const float realSecondsPerGameDay = 360f;
    // private const float rotationDegreesPerDay = 360f;
    // private const float hoursPerDay = 24f;
    // private const float minutesPerHour = 60f;

    // [SerializeField]
    // private Transform _clockHourHandTransform;

    // [SerializeField]
    // private Transform _clockMinuteHandTransform;

    // private float _day;
    // private float _dayNormalized;

    // public float HoursFloat;

    // public string MinutesString;

    // Calculate starting time
    // private void Awake()
    // {
    //     ResetClock();
    // }

    // private void Update()
    // {
    //     // Increase in-game time if game scene is active

    //     if (!MainNavigation.IsPaused || !GameManager.Instance.IsGameOver)
    //     {
    //         _day += Time.deltaTime / realSecondsPerGameDay;
    //     }

    //     _dayNormalized = _day % 1f;

    //     UpdateClock();

    //     // This value is use in the game manager for checking final objective
    //     HoursFloat = Mathf.Floor(_dayNormalized * hoursPerDay);

    //     // hour string and minute string are both used in the final game over panel
    //     MinutesString = Mathf
    //         .Floor(_dayNormalized * hoursPerDay % 1f * minutesPerHour)
    //         .ToString("00");
    // }

    // public void ResetClock()
    // {
    //     _day = hoursPerDay;
    // }

    // // Rotate clock hands
    // private void UpdateClock()
    // {
    //     _clockHourHandTransform.eulerAngles = new Vector3(
    //         0,
    //         0,
    //         -_dayNormalized * rotationDegreesPerDay
    //     );
    //     _clockMinuteHandTransform.eulerAngles = new Vector3(
    //         0,
    //         0,
    //         -_dayNormalized * rotationDegreesPerDay * hoursPerDay
    //     );
    // }
    private float _gameDaySeconds;
    private float _currentTime;

    private bool _timerActive;

    private float _startingTime;

    void Start()
    {
        // Reset in Game manager prior to new round or if level data updates
        ResetTimer();
        _startingTime = 6;
    }

    void Update()
    {
        // Increase time based on time clock and game day seconds

        // Timer only increments while game is playing (and not while paused, on main menu, or after dying)
        if (_timerActive)
        {
            // in-game time is a fraction of real time. Ex: 1 real second is .5 in-game seconds
            _currentTime += Time.deltaTime * _gameDaySeconds;
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

        // .5f in-game seconds for every 1 real time second
        _gameDaySeconds = GameManager.Instance.LevelData.GameDaySeconds;
    }

    public TimeSpan GetTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(_currentTime + _startingTime);
        return time;
    }
}
