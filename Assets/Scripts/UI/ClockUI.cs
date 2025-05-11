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
    private Transform clockHourHandTransform;

    [SerializeField]
    private Transform clockMinuteHandTransform;

    private float day;
    private float dayNormalized;

    public float hoursFloat;

    public string hoursString;
    public string minutesString;

    // Calculate starting time
    private void Awake()
    {
        day = startingTime / hoursPerDay;
    }

    private void Update()
    {
        // Increase in-game time if game scene is active

        if (!MainNavigation.isPaused || !GameManager.Instance.isGameOver)
        {
            day += Time.deltaTime / realSecondsPerGameDay;
        }

        dayNormalized = day % 1f;

        UpdateClock();

        // This value is use in the game manager for checking final objective
        hoursFloat = Mathf.Floor(dayNormalized * hoursPerDay);

        // hour string and minute string are both used in the final game over panel
        hoursString = hoursFloat.ToString("00");
        minutesString = Mathf
            .Floor(dayNormalized * hoursPerDay % 1f * minutesPerHour)
            .ToString("00");
    }

    // Rotate clock hands
    private void UpdateClock()
    {
        clockHourHandTransform.eulerAngles = new Vector3(
            0,
            0,
            -dayNormalized * rotationDegreesPerDay
        );
        clockMinuteHandTransform.eulerAngles = new Vector3(
            0,
            0,
            -dayNormalized * rotationDegreesPerDay * hoursPerDay
        );
    }
}
