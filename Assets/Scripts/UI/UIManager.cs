using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _killCountText;

    private void Update()
    {
        // Count of all enemies
        _killCountText.text = GameManager.Instance.TotalCount.ToString();
    }
}
