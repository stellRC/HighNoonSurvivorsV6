using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    private Transform playerTransform;

    private SpriteRenderer projectileSprite;

    [SerializeField]
    private GameObject impactEffect;

    public float projectileSpeed;

    private bool returned;

    private int damage;
    private Vector2 projectileTarget;

    private void Awake()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        projectileSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        returned = false;

        damage = 1;

        projectileTarget = new Vector2(playerTransform.position.x, playerTransform.position.y);
        FlipSprite();
    }

    // Return object to pool if no collision
    private void FixedUpdate()
    {
        MoveProjectile();
    }

    // Move projectile towards the player
    private void MoveProjectile()
    {
        // Rotate bullet in direction of velocity

        transform.position = Vector2.MoveTowards(
            transform.position,
            projectileTarget,
            projectileSpeed * Time.deltaTime
        );
    }

    void Update()
    {
        if (
            transform.position.x == projectileTarget.x
            && transform.position.y == projectileTarget.y
            && !returned
        )
        {
            ObjectPooling.ReturnObjectToPool(gameObject);
            returned = true;
        }
    }

    private void ReturnToPool()
    {
        ObjectPooling.ReturnObjectToPool(gameObject);
        returned = true;
    }

    // Flip the sprite to face direction of player
    private void FlipSprite()
    {
        if (playerTransform.position.x > transform.position.x)
        {
            projectileSprite.flipX = false;
        }
        else
        {
            projectileSprite.flipX = true;
        }
    }

    // Destroy projectile if touching player and projectile has not already been destroyed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();

        if (returned)
        {
            return;
        }

        if (collision.gameObject.name == "PlayerCharacter" && !GameManager.Instance.noDamage)
        {
            // Instantiate(impactEffect, transform.position, Quaternion.identity);
            iDoDamage?.DoDamage(damage);

            ReturnToPool();
        }
    }

    // Lighting and fog collision
    public void NonPlayerCollision()
    {
        ObjectPooling.ReturnObjectToPool(gameObject);
        GameManager.Instance.projectileCount += 1;
        returned = true;
    }
}
