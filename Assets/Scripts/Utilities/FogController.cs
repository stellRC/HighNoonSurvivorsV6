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
}
