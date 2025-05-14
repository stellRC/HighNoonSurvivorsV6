using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    [SerializeField]
    private GameObject _popupMenu;
    private readonly LeanTweenType _inType;
    private readonly LeanTweenType _outType;
    private float _duration;
    private float _delay;

    void Start()
    {
        _duration = 0.3f;
        _delay = 0.1f;
    }

    public void OnOpen()
    {
        _popupMenu.transform.localScale = new Vector3(0, 0, 0);
        LeanTween
            .scale(_popupMenu, new Vector3(1, 1, 1), _duration)
            .setDelay(_delay)
            .setEase(_inType);
    }

    public void OnClose()
    {
        _popupMenu.transform.localScale = new Vector3(0, 0, 0);
        LeanTween
            .scale(_popupMenu, new Vector3(0, 0, 0), _duration)
            .setDelay(_delay)
            .setEase(_outType);
    }
}
