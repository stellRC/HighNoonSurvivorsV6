using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    private VisualEffect _vfxRenderer;

    private PlayerMovement _playerPosition;

    private Enemy _enemyPosition;

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
