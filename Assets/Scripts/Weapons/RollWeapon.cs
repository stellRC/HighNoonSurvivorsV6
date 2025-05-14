using UnityEngine;

public class RollWeapon : WeaponBase
{
    private Enemy enemy;

    void OnEnable()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();

        if (collision.gameObject.name == "PlayerCharacter" && !GameManager.Instance.NoDamage)
        {
            Debug.Log("player hit");
            iDoDamage?.DoDamage(Damage);
            // enemy explodes on impact
            enemy.DoDamage(Damage);
        }
    }
}
