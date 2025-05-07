using UnityEngine;
using UnityEngine.VFX;

public class FogController : MonoBehaviour
{
    [SerializeField]
    private VisualEffect vfxRenderer;

    void Update()
    {
        // Control collision with fog layer (foreground)
        if (vfxRenderer != null)
        {
            vfxRenderer.SetVector3("ColliderPosition", transform.position);
        }
    }
}
