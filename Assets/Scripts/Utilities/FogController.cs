using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    [SerializeField]
    private VisualEffect vfxRenderer;
    private PlayerMovement playerPosition;

    void Awake()
    {
        playerPosition = FindAnyObjectByType<PlayerMovement>();
    }

    void Update()
    {
        // Control collision with fog layer (foreground)
        if (vfxRenderer != null)
        {
            vfxRenderer.SetVector3("ColliderPosition", playerPosition.transform.position);
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
