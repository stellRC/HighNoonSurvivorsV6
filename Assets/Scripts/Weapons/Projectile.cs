using UnityEngine;

public class Projectile : WeaponBase
{
    private Transform _playerTransform;

    private SpriteRenderer _projectileSprite;

    [SerializeField]
    private float _projectileSpeed;

    [SerializeField]
    [Tooltip("Check if returned to pool")]
    private bool _returned;

    private Vector2 _projectileTarget;

    private void Awake()
    {
        _playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        _projectileSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _returned = false;

        _projectileSpeed = 6.5f;

        _projectileTarget = new Vector2(_playerTransform.position.x, _playerTransform.position.y);
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
            _projectileTarget,
            _projectileSpeed * Time.deltaTime
        );
    }

    void Update()
    {
        if (
            transform.position.x == _projectileTarget.x
            && transform.position.y == _projectileTarget.y
            && !_returned
        )
        {
            ObjectPooling.ReturnObjectToPool(gameObject);
            _returned = true;
        }
    }

    private void ReturnToPool()
    {
        ObjectPooling.ReturnObjectToPool(gameObject);
        _returned = true;
    }

    // Flip the sprite to face direction of player
    private void FlipSprite()
    {
        if (_playerTransform.position.x > transform.position.x)
        {
            _projectileSprite.flipX = false;
        }
        else
        {
            _projectileSprite.flipX = true;
        }
    }

    // Destroy projectile if touching player and projectile has not already been destroyed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();

        if (_returned)
        {
            return;
        }

        if (collision.gameObject.name == "PlayerCharacter" && !GameManager.Instance.NoDamage)
        {
            // Instantiate(impactEffect, transform.position, Quaternion.identity);
            iDoDamage?.DoDamage(Damage);

            ReturnToPool();
        }
    }

    // Lighting and fog collision
    public void NonPlayerCollision()
    {
        ObjectPooling.ReturnObjectToPool(gameObject);
        GameManager.Instance.ProjectileCount += 1;
        _returned = true;
    }
}
