using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class LightningCollision : MonoBehaviour
{
    private EdgeCollider2D edgeCollider2D;
    private LightningBoltScript lightningLine;
    private LineRenderer lineRenderer;

    void Start()
    {
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        lightningLine = GetComponent<LightningBoltScript>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!lightningLine.ManualMode)
        {
            edgeCollider2D.enabled = true;
            SetEdgeCollider(lineRenderer);
        }
        else if (edgeCollider2D.enabled)
        {
            edgeCollider2D.Reset();
            edgeCollider2D.enabled = false;
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

        edgeCollider2D.SetPoints(edges);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If a missile hits this object
        if (collision.transform.CompareTag("brawler") || collision.transform.CompareTag("gunman"))
        {
            collision.GetComponent<Enemy>().DoDamage(1);
        }
        else if (collision.transform.CompareTag("projectile"))
        {
            collision.GetComponent<Projectile>().NonPlayerCollision();
        }
    }
}
