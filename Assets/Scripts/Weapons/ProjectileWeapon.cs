using System.Collections;
using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private Transform attackPoint;

    private MasterAnimator enemyAnimation;

    private float waitTime;

    private bool playAudioOnce;

    private float attackRate;

    void Awake()
    {
        enemyAnimation = GetComponent<MasterAnimator>();
    }

    private void OnEnable()
    {
        playAudioOnce = false;
        attackRate = 0;
    }

    private void Update()
    {
        attackRate += Time.deltaTime;
        CheckIfCanAttack();
    }

    // Limit attack rate based on random range setting
    private void CheckIfCanAttack()
    {
        if (attackRate >= waitTime)
        {
            EnemyAttack();
            waitTime = Random.Range(1f, 4f);
            attackRate = 0;
        }
    }

    private void EnemyAttack()
    {
        var attack = Random.Range(0, 4);

        enemyAnimation.ChangeAnimation(enemyAnimation.projectileAnimation[attack]);

        // Prevent audio from playing every instance since it can become annoying
        // attack in this context has no meaning other than as a limiting factor
        if (!playAudioOnce && attack == 0 && !GameManager.Instance.playerDead)
        {
            ProjectileAudio();
        }
    }

    private void ProjectileAudio()
    {
        SoundEffectsManager.instance.PlayRandomSoundFXClip(
            SoundEffectsManager.instance.shootingSoundClips,
            transform,
            .75f
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();

        // Only collide with player if they are able to take damage (and don't currently have invincibility skill)

        if (collision.gameObject.name == "PlayerCharacter" && !GameManager.Instance.noDamage)
        {
            playAudioOnce = true;

            iDoDamage?.DoDamage(damage);
        }
    }

    // Instantiate bullet object from prefab at enemy position
    public void InstantiateProjectile(Vector2 position)
    {
        // adjust for sprite offset
        position = new Vector2(position.x, position.y - .5f);

        ObjectPooling.SpawnObject(
            enemyData.projectilePrefab,
            position,
            Quaternion.identity,
            ObjectPooling.PoolType.Projectiles
        );
    }
}
