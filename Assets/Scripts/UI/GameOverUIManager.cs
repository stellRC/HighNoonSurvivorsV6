using TMPro;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text totalCountText;

    [SerializeField]
    private TMP_Text brawlerCountText;

    [SerializeField]
    private TMP_Text projectileCountText;

    [SerializeField]
    private TMP_Text gunmanCountText;

    [SerializeField]
    private TMP_Text timeText;

    private ClockUI clockUI;

    void Awake()
    {
        clockUI = FindAnyObjectByType<ClockUI>();
    }

    public void OnGameOver()
    {
        totalCountText.text = "Total Enemies: " + GameManager.Instance.totalCount;
        brawlerCountText.text = "Total Brawlers: " + GameManager.Instance.brawlerCount;
        projectileCountText.text = "Total Projectiles: " + GameManager.Instance.projectileCount;
        gunmanCountText.text = "Total Shooters: " + GameManager.Instance.gunmanCount;
        timeText.text = "Time Survived: " + clockUI.hoursString + ":" + clockUI.minutesString;
    }
}
