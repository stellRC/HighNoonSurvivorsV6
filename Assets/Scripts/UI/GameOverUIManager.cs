using TMPro;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private TMP_Text totalCountText;

    [SerializeField]
    private TMP_Text brawlerCountText;

    [SerializeField]
    private TMP_Text projectileCountText;

    [SerializeField]
    private TMP_Text gunmanCountText;

    [SerializeField]
    private TMP_Text bonusCountText;

    [SerializeField]
    private TMP_Text timeText;

    void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    public void OnGameOver()
    {
        totalCountText.text = "Total Enemies: " + gameManager.totalCount;
        brawlerCountText.text = "Total Enemies: " + gameManager.brawlerCount;
        projectileCountText.text = "Total Enemies: " + gameManager.projectileCount;
        gunmanCountText.text = "Total Enemies: " + gameManager.gunmanCount;
        timeText.text = "Time Survived: " + gameManager.timeCount;
    }
}
