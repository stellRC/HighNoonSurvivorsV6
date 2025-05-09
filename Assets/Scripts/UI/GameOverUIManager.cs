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
    private TMP_Text timeText;

    void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    public void OnGameOver()
    {
        totalCountText.text = "Total Enemies: " + gameManager.totalCount;
        brawlerCountText.text = "Total Brawlers: " + gameManager.brawlerCount;
        projectileCountText.text = "Total Projectiles: " + gameManager.projectileCount;
        gunmanCountText.text = "Total Shooters: " + gameManager.gunmanCount;
        timeText.text = "Time Survived: " + gameManager.timeCount;
    }
}
