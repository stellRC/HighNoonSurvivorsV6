using UnityEngine;

public class RollWeapon : WeaponBase
{
    [SerializeField]
    private EnemyData enemyData;

    private Enemy enemy;

    void OnEnable()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();

        if (collision.gameObject.name == "PlayerCharacter" && !GameManager.Instance.noDamage)
        {
            Debug.Log("player hit");
            iDoDamage?.DoDamage(damage);
            // enemy explodes on impact
            enemy.DoDamage(damage);
        }
    }
}
