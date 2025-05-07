using TMPro;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private TMP_Text killCountText;

    [SerializeField]
    private TMP_Text timeText;

    // [SerializeField]
    // private TMP_Text streakText;

    [SerializeField]
    private ClockUI clockUI;

    public void OnGameOver()
    {
        killCountText.text = "Kill Count: " + gameManager.killCount;
        timeText.text = "Alive: " + clockUI.hoursString + ":" + clockUI.minutesString;
    }
}
