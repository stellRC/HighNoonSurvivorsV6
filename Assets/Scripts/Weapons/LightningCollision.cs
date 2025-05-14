using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class LightningCollision : MonoBehaviour
{
    private EdgeCollider2D _edgeCollider2D;
    private LightningBoltScript _lightningLine;
    private LineRenderer _lineRenderer;

    void Start()
    {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        _lightningLine = GetComponent<LightningBoltScript>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!_lightningLine.ManualMode)
        {
            _edgeCollider2D.enabled = true;
            SetEdgeCollider(_lineRenderer);
        }
        else if (_edgeCollider2D.enabled)
        {
            _edgeCollider2D.Reset();
            _edgeCollider2D.enabled = false;
        }
    }

    private void SetEdgeCollider(LineRenderer lineRenderer)
    {
        List<Vector2> edges = new();

        for (int point = 0; point < lineRenderer.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRenderer.GetPosition(point);
            edges.Add(lineRendererPoint);
        }

        _edgeCollider2D.SetPoints(edges);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If a missile hits this object
        if (
            collision.transform.CompareTag("brawler")
            || collision.transform.CompareTag("gunman")
            || collision.transform.CompareTag("roller")
        )
        {
            collision.GetComponent<Enemy>().DoDamage(1);
        }
        else if (collision.transform.CompareTag("projectile"))
        {
            collision.GetComponent<Projectile>().NonPlayerCollision();
        }
    }
}
