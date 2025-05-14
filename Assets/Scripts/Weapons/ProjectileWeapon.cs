using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [SerializeField]
    private EnemyData _enemyData;

    private MasterAnimator _enemyAnimation;
    private float _waitTime;

    private bool _playAudioOnce;

    private float _attackRate;

    void Awake()
    {
        _enemyAnimation = GetComponent<MasterAnimator>();
    }

    private void OnEnable()
    {
        _playAudioOnce = false;
        _attackRate = 0;
    }

    private void Update()
    {
        _attackRate += Time.deltaTime;
        CheckIfCanAttack();
    }

    // Limit attack rate based on random range setting
    private void CheckIfCanAttack()
    {
        // Ensure enemy is in camera viewport
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (
            _attackRate >= _waitTime
            && viewPos.x >= 0.05f
            && viewPos.x <= .95f
            && viewPos.y >= 0.05f
            && viewPos.y <= .95f
        )
        {
            EnemyAttack();
            _waitTime = Random.Range(1f, 4f);
            _attackRate = 0;
        }
    }

    private void EnemyAttack()
    {
        var attackID = Random.Range(0, 4);

        _enemyAnimation.ChangeAnimation(_enemyAnimation.ProjectileAnimation[attackID]);

        // Prevent audio from playing every instance since it can become annoying
        // attack in this context has no meaning other than as a limiting factor
        if (!_playAudioOnce && attackID == 0 && !GameManager.Instance.PlayerDead)
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

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();

    //     // Only collide with player if they are able to take damage (and don't currently have invincibility skill)

    //     if (collision.gameObject.name == "PlayerCharacter" && !GameManager.Instance.noDamage)
    //     {
    //         _playAudioOnce = true;

    //         iDoDamage?.DoDamage(Damage);
    //     }
    // }

    // Instantiate bullet object from prefab at enemy position
    public void InstantiateProjectile(Vector2 position)
    {
        // adjust for sprite offset
        position = new Vector2(position.x, position.y - .5f);

        ObjectPooling.SpawnObject(
            _enemyData.ProjectilePrefab,
            position,
            Quaternion.identity,
            ObjectPooling.PoolType.Projectiles
        );
    }
}
