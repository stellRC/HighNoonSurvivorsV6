using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    private Button _gitBtn;

    void Awake()
    {
        _gitBtn.onClick.AddListener(OpenLink);
    }

    public void OpenLink()
    {
        Application.OpenURL("https://github.com/stellRC/HighNoonSurvivorsV6");
    }
}
