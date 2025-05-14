using UnityEngine;

public class ParticleBurst : WeaponBase
{
    private bool _once;

    void OnEnable()
    {
        _once = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();
        var sprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();

        if (
            collision.gameObject.name == "PlayerCharacter"
            && !GameManager.Instance.NoDamage
            && _once
            && sprite.enabled == false
        )
        {
            _once = false;
            iDoDamage?.DoDamage(Damage);
        }
    }
}
