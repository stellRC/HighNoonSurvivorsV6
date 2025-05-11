using System.Collections;
using UnityEngine;

public class EnemyWeapon : WeaponBase
{
    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private Transform attackPoint;

    private MasterAnimator enemyAnimation;

    private Transform playerTransform;

    private float waitTime;

    void Awake()
    {
        enemyAnimation = GetComponent<MasterAnimator>();
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (
            Vector2.Distance(transform.position, playerTransform.position)
            <= enemyData.minimumDistance
        )
        {
            EnemyAttack();
            waitTime = Random.Range(1.5f, 4);
            StartCoroutine(EnemyAttackRate(waitTime));
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
        if (enemyData.name == "brawl")
        {
            enemyAnimation.ChangeAnimation(enemyAnimation.brawlAnimation[attack]);
        }
        else
        {
            enemyAnimation.ChangeAnimation(enemyAnimation.projectileAnimation[attack]);
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
            iDoDamage?.DoDamage(damage);
            // isTriggered = true;
        }
    }

    public void InstantiateProjectile(Vector2 position)
    {
        if (enemyData.name != "brawl")
        {
            ObjectPooling.SpawnObject(
                enemyData.projectilePrefab,
                position,
                Quaternion.identity,
                ObjectPooling.PoolType.Projectiles
            );
        }
    }
}
