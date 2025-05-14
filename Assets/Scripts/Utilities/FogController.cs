using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    private VisualEffect _vfxRenderer;

    private PlayerMovement _playerPosition;

    public bool IsPlaying;

    void Awake()
    {
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If a missile hits this object
        if (collision.transform.CompareTag("brawler") || collision.transform.CompareTag("gunman"))
        {
            Debug.Log("fog: " + collision);
            collision.GetComponent<Enemy>().DoDamage(1);
        }
        else if (collision.transform.CompareTag("projectile"))
        {
            Debug.Log("fog: " + collision);
            collision.GetComponent<Projectile>().NonPlayerCollision();
        }
    }
}
