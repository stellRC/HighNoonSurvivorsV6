using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text killCountText;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = gameObject.GetComponentInParent<GameManager>();
    }

    private void Update()
    {
        killCountText.text = gameManager.totalCount.ToString();
    }
}
