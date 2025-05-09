using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    private VisualEffect vfxRenderer;

    private PlayerMovement playerPosition;

    public bool isPlaying;

    private Vector2 colliderPosition;

    void Awake()
    {
        playerPosition = FindFirstObjectByType<PlayerMovement>();
        vfxRenderer = FindFirstObjectByType<VisualEffect>();
        vfxRenderer.Stop();
        isPlaying = false;
    }

    void Update()
    {
        // Control collision with fog layer (foreground)
        if (playerPosition == null)
        {
            playerPosition = FindFirstObjectByType<PlayerMovement>();
            vfxRenderer = FindFirstObjectByType<VisualEffect>();
            vfxRenderer.Stop();
            isPlaying = false;
        }

        if (vfxRenderer != null)
        {
            if (colliderPosition == null)
            {
                colliderPosition = GameObject.FindGameObjectWithTag("brawler").transform.position;
            }
            vfxRenderer.SetVector3("ColliderPosition", colliderPosition);

            if (isPlaying)
            {
                vfxRenderer.Play();
            }
            else
            {
                vfxRenderer.Stop();
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
