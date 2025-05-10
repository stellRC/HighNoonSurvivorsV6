using UnityEngine;

public class Enemy : MonoBehaviour, IDoDamage
{
    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private SpriteRenderer enemySprite;

    private EnemyManager enemyManager;

    private ParticleSystem deathParticleSystem;

    private MasterAnimator enemyAnimation;

    public float currentHealth;

    public bool isDead;

    private bool IsPlayingParticles;

    void Awake()
    {
        deathParticleSystem = GetComponent<ParticleSystem>();

        enemyAnimation = GetComponent<MasterAnimator>();
        enemyManager = FindAnyObjectByType<EnemyManager>();
    }

    void OnEnable()
    {
        isDead = false;
        currentHealth = enemyData.maxHealth;
        enemySprite.enabled = true;
        Physics2D.IgnoreCollision(
            GetComponent<BoxCollider2D>(),
            GetComponentInChildren<BoxCollider2D>()
        );
    }

    public void Update()
    {
        if (IsPlayingParticles && deathParticleSystem.isStopped)
        {
            enemyManager.SpawnMoreEnemies();
            deathParticleSystem.Stop();
            IsPlayingParticles = false;
            ReturnToPool();
        }
    }

    public void DoDamage(int damage)
    {
        if (gameObject != null)
        {
            HurtAnimation();
            // Decrease health based on damage
            currentHealth -= damage;

            // play hurt animation
            if (currentHealth <= 0)
            {
                isDead = true;
                Die();
            }
        }
    }

    private void HurtAnimation()
    {
        // // Hurt animation
        enemyAnimation.ChangeAnimation(enemyAnimation.stateAnimation[4]);
    }

    private void Die()
    {
        DeathAnimation();
        UpdateStats();
        DisableComponents();
    }

    private void UpdateStats()
    {
        GameManager.Instance.totalCount += 1;
        if (transform.CompareTag("brawler"))
        {
            GameManager.Instance.brawlerCount++;
        }
        else if (transform.CompareTag("gunman"))
        {
            GameManager.Instance.gunmanCount++;
        }
        else if (transform.CompareTag("roller"))
        {
            GameManager.Instance.rollerCount++;
        }
    }

    public void DeathAnimation()
    {
        enemyAnimation.ChangeAnimation(enemyAnimation.stateAnimation[4]);
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
        GetComponent<Collider2D>().enabled = false;
    }

    // Enabled when Death animation finishes via Particle Death Animation script
    public void EnableParticles()
    {
        var emission = deathParticleSystem.emission;

        emission.enabled = true;
        if (!IsPlayingParticles)
        {
            deathParticleSystem.Play();
            enemySprite.enabled = false;
            IsPlayingParticles = true;
        }
    }

    public void ReturnToPool()
    {
        if (deathParticleSystem.isStopped)
        {
            ObjectPooling.ReturnObjectToPool(gameObject);
        }
    }
}
