using System.Collections;
using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private Transform attackPoint;

    private MasterAnimator enemyAnimation;

    private Transform playerTransform;

    private float waitTime;

    private bool playOnce;

    void Awake()
    {
        enemyAnimation = GetComponent<MasterAnimator>();
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
    }

    private void OnEnable()
    {
        playOnce = false;
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
        if (!playOnce)
        {
            ProjectileAudio();
        }

        enemyAnimation.ChangeAnimation(enemyAnimation.projectileAnimation[attack]);
    }

    private void ProjectileAudio()
    {
        SoundEffectsManager.instance.PlayRandomSoundFXClip(
            SoundEffectsManager.instance.shootingSoundClips,
            transform,
            1f
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();

        if (collision.gameObject.name == "PlayerCharacter" && !GameManager.Instance.noDamage)
        {
            playOnce = true;

            iDoDamage?.DoDamage(damage);
        }
    }

    public void InstantiateProjectile(Vector2 position)
    {
        // adjust offset
        position = new Vector2(position.x + -.5f, position.y - .5f);

        ObjectPooling.SpawnObject(
            enemyData.projectilePrefab,
            position,
            Quaternion.identity,
            ObjectPooling.PoolType.Projectiles
        );
    }
}
