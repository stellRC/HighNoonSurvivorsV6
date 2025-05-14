using UnityEngine;

public class ClockUI : MonoBehaviour
{
    // an in-game day is 6 minutes long
    private const float realSecondsPerGameDay = 360f;
    private const float rotationDegreesPerDay = 360f;
    private const float hoursPerDay = 24f;
    private const float minutesPerHour = 60f;

    private const float startingTime = 6f; //6:00am

    [SerializeField]
    private Transform _clockHourHandTransform;

    [SerializeField]
    private Transform _clockMinuteHandTransform;

    private float _day;
    private float _dayNormalized;

    public float HoursFloat;

    public string HoursString;
    public string MinutesString;

    // Calculate starting time
    private void Awake()
    {
        _day = startingTime / hoursPerDay;
    }

    private void Update()
    {
        // Increase in-game time if game scene is active

        if (!MainNavigation.IsPaused || !GameManager.Instance.IsGameOver)
        {
            _day += Time.deltaTime / realSecondsPerGameDay;
        }

        _dayNormalized = _day % 1f;

        UpdateClock();

        // This value is use in the game manager for checking final objective
        HoursFloat = Mathf.Floor(_dayNormalized * hoursPerDay);

        // hour string and minute string are both used in the final game over panel
        HoursString = HoursFloat.ToString("00");
        MinutesString = Mathf
            .Floor(_dayNormalized * hoursPerDay % 1f * minutesPerHour)
            .ToString("00");
    }

    // Rotate clock hands
    private void UpdateClock()
    {
        _clockHourHandTransform.eulerAngles = new Vector3(
            0,
            0,
            -_dayNormalized * rotationDegreesPerDay
        );
        _clockMinuteHandTransform.eulerAngles = new Vector3(
            0,
            0,
            -_dayNormalized * rotationDegreesPerDay * hoursPerDay
        );
    }
}
