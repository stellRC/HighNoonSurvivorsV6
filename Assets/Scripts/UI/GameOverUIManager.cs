using TMPro;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private TMP_Text enemyCountText;

    [SerializeField]
    private TMP_Text bonusCountText;

    [SerializeField]
    private TMP_Text timeText;

    private ClockUI clockUI;

    void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
        clockUI = GetComponent<ClockUI>();
    }

    public void OnGameOver()
    {
        enemyCountText.text = "Kill Count: " + gameManager.killCount;
        timeText.text = "Alive: " + clockUI.hoursString + ":" + clockUI.minutesString;
    }
}
