using TMPro;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _titleText;

    [SerializeField]
    private TMP_Text _totalCountText;

    [SerializeField]
    private TMP_Text _brawlerCountText;

    [SerializeField]
    private TMP_Text _gunmanCountText;

    [SerializeField]
    private TMP_Text _rollerCountText;

    [SerializeField]
    private TMP_Text _timeText;

    private ClockUI clockUI;

    private int _startingTime;

    private float _endingTime;

    void Awake()
    {
        clockUI = FindAnyObjectByType<ClockUI>();
        _startingTime = 6;
    }

    public void OnGameOver()
    {
        _endingTime = GameManager.Instance.ClockUI.HoursFloat + _startingTime;
        if (_endingTime >= 12)
        {
            _titleText.text = "Game Over: You Survived Past Noon!";
        }
        else
        {
            _titleText.text = "Game Over: Better Luck Next Round!";
        }

        _totalCountText.text = "Total Enemies: " + GameManager.Instance.TotalCount;
        _brawlerCountText.text = "Brawlers: " + GameManager.Instance.BrawlerCount;
        _rollerCountText.text = "Rollers: " + GameManager.Instance.RollerCount;
        _gunmanCountText.text = "Shooters: " + GameManager.Instance.GunmanCount;
        _timeText.text = "Time of Death: " + _endingTime.ToString() + ":" + clockUI.MinutesString;
    }
}
