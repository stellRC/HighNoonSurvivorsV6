using TMPro;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
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

    void Awake()
    {
        clockUI = FindAnyObjectByType<ClockUI>();
    }

    public void OnGameOver()
    {
        _totalCountText.text = "Total Enemies: " + GameManager.Instance.TotalCount;
        _brawlerCountText.text = "Brawlers: " + GameManager.Instance.BrawlerCount;
        _rollerCountText.text = "Rollers: " + GameManager.Instance.RollerCount;

        _gunmanCountText.text = "Shooters: " + GameManager.Instance.GunmanCount;
        _timeText.text = "Time of Death: " + clockUI.HoursString + ":" + clockUI.MinutesString;
    }
}
