using UnityEngine;

public class Enemy : MonoBehaviour, IDoDamage
{
    [SerializeField]
    private EnemyData _enemyData;

    [SerializeField]
    private SpriteRenderer _enemySprite;

    private EnemyManager _enemyManager;

    private ParticleSystem _deathParticleSystem;

    private MasterAnimator _enemyAnimation;

    private float _currentHealth;

    public bool IsDead;

    private bool _isPlayingParticles;

    public Vector3 CurrentPosition;

    void Awake()
    {
        _deathParticleSystem = GetComponent<ParticleSystem>();

        _enemyAnimation = GetComponent<MasterAnimator>();
        _enemyManager = FindAnyObjectByType<EnemyManager>();
    }

    void OnEnable()
    {
        IsDead = false;
        _currentHealth = _enemyData.MaxHealth;
        EnableComponents();
        Physics2D.IgnoreCollision(
            GetComponent<BoxCollider2D>(),
            GetComponentInChildren<BoxCollider2D>()
        );
    }

    public void Update()
    {
        CurrentPosition = Camera.main.WorldToViewportPoint(transform.position);
        // Avoid error by stopping particle emission
        if (_isPlayingParticles && _deathParticleSystem.isStopped)
        {
            DespawnSet();
            ReturnToPool();
        }
    }

    private void DespawnSet()
    {
        _enemyManager.SpawnMoreEnemies();
        _deathParticleSystem.Stop();
        _isPlayingParticles = false;
    }

    public void DoDamage(int damage)
    {
        if (gameObject != null)
        {
            HurtAnimation();
            // Decrease health based on damage
            _currentHealth -= damage;

            // play hurt animation
            if (_currentHealth <= 0)
            {
                IsDead = true;
                Die();
            }
        }
    }

    private void HurtAnimation()
    {
        // // Hurt animation
        _enemyAnimation.ChangeAnimation(_enemyAnimation.StateAnimation[4]);
    }

    private void Die()
    {
        DeathAnimation();
        UpdateStats();
        DisableComponents();
    }

    private void UpdateStats()
    {
        GameManager.Instance.TotalCount += 1;
        if (transform.CompareTag("brawler"))
        {
            GameManager.Instance.BrawlerCount++;
        }
        else if (transform.CompareTag("gunman"))
        {
            GameManager.Instance.GunmanCount++;
        }
        else if (transform.CompareTag("roller"))
        {
            GameManager.Instance.RollerCount++;
        }
    }

    public void DeathAnimation()
    {
        _enemyAnimation.ChangeAnimation(_enemyAnimation.StateAnimation[4]);
        EnableParticles();
    }

    private void DisableComponents()
    {
        // Prevent collision with dead enemy
        // Disable movement and animation updates
        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }
        else if (GetComponent<ProjectileWeapon>() != null)
        {
            GetComponent<ProjectileWeapon>().enabled = false;
        }
    }

    private void EnableComponents()
    {
        GetComponent<Collider2D>().enabled = true;
        if (GetComponent<ProjectileWeapon>() != null)
        {
            GetComponent<ProjectileWeapon>().enabled = true;
        }
        _enemySprite.enabled = true;
    }

    // Enabled when Death animation finishes via Particle Death Animation script
    public void EnableParticles()
    {
        var emission = _deathParticleSystem.emission;
        if (!GameManager.Instance.PlayerDead)
        {
            EnemyDeathAudio();
        }

        emission.enabled = true;

        if (!_isPlayingParticles)
        {
            _deathParticleSystem.Play();
            // Hide sprite prior to particles
            _enemySprite.enabled = false;
            _isPlayingParticles = true;
        }
    }

    private void EnemyDeathAudio()
    {
        SoundEffectsManager.Instance.PlayRandomSoundFXClip(
            SoundEffectsManager.Instance.particleDeathSoundClips,
            transform,
            .2f
        );
    }

    public void ReturnToPool()
    {
        if (_deathParticleSystem.isStopped)
        {
            ObjectPooling.ReturnObjectToPool(gameObject);
        }
    }
}
