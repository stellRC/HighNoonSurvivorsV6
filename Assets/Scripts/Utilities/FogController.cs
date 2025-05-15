using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _fogBackground;

    [SerializeField]
    private SpriteRenderer _fogForeground;
    private VisualEffect _vfxRenderer;

    private ClockUI _clockUI;

    private PlayerMovement _playerPosition;

    public bool IsPlaying;
    private Color _fogColor;

    void Awake()
    {
        _clockUI = GetComponentInChildren<ClockUI>();
        _playerPosition = FindFirstObjectByType<PlayerMovement>();
        _vfxRenderer = FindFirstObjectByType<VisualEffect>();
        _vfxRenderer.Stop();
        IsPlaying = false;
    }

    void Update()
    {
        // Control collision with fog layer (foreground)
        if (_playerPosition == null)
        {
            _playerPosition = FindFirstObjectByType<PlayerMovement>();
            _vfxRenderer = FindFirstObjectByType<VisualEffect>();
            _fogBackground = GameObject.Find("FogBackground").GetComponent<SpriteRenderer>();
            _fogForeground = GameObject.Find("FogForeground").GetComponent<SpriteRenderer>();

            _vfxRenderer.Stop();
            IsPlaying = false;
        }

        if (_vfxRenderer != null)
        {
            _vfxRenderer.SetVector3("ColliderPosition", _playerPosition.transform.position);
            if (IsPlaying)
            {
                _vfxRenderer.Play();
            }
            else
            {
                _vfxRenderer.Stop();
            }
        }
    }

    private void UpdateFog()
    {
        _fogColor.a = _clockUI.GetTime().Hours / 12;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var damage = 1;

        // If a missile hits this object
        if (
            collision.transform.CompareTag("brawler")
            || collision.transform.CompareTag("gunman")
            || collision.transform.CompareTag("roller")
        )
        {
            Debug.Log("fog collision");
            collision.GetComponent<Enemy>().DoDamage(damage);
        }
        else if (collision.transform.CompareTag("projectile"))
        {
            collision.GetComponent<Projectile>().NonPlayerCollision();
        }
    }
}
