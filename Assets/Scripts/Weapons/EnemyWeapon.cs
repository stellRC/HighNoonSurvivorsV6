using System.Collections;
using UnityEngine;

public class EnemyWeapon : WeaponBase
{
    [SerializeField]
    private EnemyData _enemyData;

    private MasterAnimator _enemyAnimation;

    private Transform _playerTransform;

    private float _waitTime;

    void Awake()
    {
        _enemyAnimation = GetComponent<MasterAnimator>();
        _playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (
            Vector2.Distance(transform.position, _playerTransform.position)
            <= _enemyData.MinimumDistance
        )
        {
            EnemyAttack();
            _waitTime = Random.Range(1.5f, 4);
            StartCoroutine(EnemyAttackRate(_waitTime));
        }
    }

    IEnumerator EnemyAttackRate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EnemyAttack();
    }

    private void EnemyAttack()
    {
        var attack = Random.Range(0, 4);
        // Shoot animation
        if (_enemyData.name == "brawl")
        {
            _enemyAnimation.ChangeAnimation(_enemyAnimation.BrawlAnimation[attack]);
        }
        else
        {
            _enemyAnimation.ChangeAnimation(_enemyAnimation.ProjectileAnimation[attack]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (isTriggered)
        //     return;
        // Damage Enemy
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();

        if (collision.gameObject.name == "PlayerCharacter")
        {
            Debug.Log("player hit");
            iDoDamage?.DoDamage(Damage);
            // isTriggered = true;
        }
    }

    public void InstantiateProjectile(Vector2 position)
    {
        if (_enemyData.name != "brawl")
        {
            ObjectPooling.SpawnObject(
                _enemyData.ProjectilePrefab,
                position,
                Quaternion.identity,
                ObjectPooling.PoolType.Projectiles
            );
        }
    }
}
