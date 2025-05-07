using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    [SerializeField]
    private GameObject popupMenu;
    public LeanTweenType inType;
    public LeanTweenType outType;
    public float duration;
    public float delay;

    void Start()
    {
        duration = 0.3f;
        delay = 0.1f;
    }

    public void OnOpen()
    {
        popupMenu.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(popupMenu, new Vector3(1, 1, 1), duration).setDelay(delay).setEase(inType);
    }

    public void OnClose()
    {
        LeanTween.scale(popupMenu, new Vector3(0, 0, 0), duration).setDelay(delay).setEase(outType);
    }
}
