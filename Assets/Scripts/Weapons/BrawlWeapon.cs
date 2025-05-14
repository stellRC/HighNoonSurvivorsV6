using UnityEngine;

public class BrawlWeapon : WeaponBase
{
    [SerializeField]
    private EnemyData enemyData;

    private MasterAnimator enemyAnimation;

    private Transform playerTransform;

    private float timeBetweenShots = 0;
    private float nextShotTime;

    void Awake()
    {
        enemyAnimation = GetComponent<MasterAnimator>();
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (
            Vector2.Distance(transform.position, playerTransform.position)
            <= enemyData.MinimumDistance
        )
        {
            // Check if can shoot
            if (Time.time > nextShotTime && enemyAnimation.AnimationFinished)
            {
                var attack = Random.Range(0, 4);
                // Shoot animation
                enemyAnimation.ChangeAnimation(enemyAnimation.BrawlAnimation[attack]);

                // Reset timer
                timeBetweenShots = Random.Range(1.5f, 5.0f);
                nextShotTime = Time.time + timeBetweenShots;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (isTriggered)
        //     return;
        // Damage Enemy
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();

        if (collision.gameObject.name == "PlayerCharacter" && !GameManager.Instance.NoDamage)
        {
            Debug.Log("player hit");
            iDoDamage?.DoDamage(Damage);
            // isTriggered = true;
        }
    }
}
