using UnityEngine;

public class ClockUI : MonoBehaviour
{
    private const float secondsPerGameDay = 1200f;
    private const float rotationDegreesPerDay = 360f;
    private const float hoursPerDay = 24f;
    private const float minutesPerHour = 60f;

    private const float startingTime = 6f; //6:00am

    private GameManager gameManager;

    private Transform clockHourHandTransform;
    private Transform clockMinuteHandTransform;

    private float day;
    private float dayNormalized;

    public string hoursString;
    public string minutesString;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        clockHourHandTransform = transform.Find("HourHand");
        clockMinuteHandTransform = transform.Find("MinuteHand");
        day = startingTime / hoursPerDay;
    }

    private void Update()
    {
        if (!MainNavigation.isPaused || !gameManager.isGameOver)
        {
            day += Time.deltaTime / secondsPerGameDay;
        }

        dayNormalized = day % 1f;

        UpdateClock();

        hoursString = Mathf.Floor(dayNormalized * hoursPerDay).ToString("00");
        minutesString = Mathf
            .Floor(dayNormalized * hoursPerDay % 1f * minutesPerHour)
            .ToString("00");
    }

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
