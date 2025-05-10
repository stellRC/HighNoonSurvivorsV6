using UnityEngine;

public class ParticleBurst : WeaponBase
{
    private bool once;

    void OnEnable()
    {
        once = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDoDamage iDoDamage = collision.gameObject.GetComponent<IDoDamage>();
        var sprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();

        if (
            collision.gameObject.name == "PlayerCharacter"
            && !GameManager.Instance.noDamage
            && once
            && sprite.enabled == false
        )
        {
            Debug.Log("player hit by particle");
            once = false;
            iDoDamage?.DoDamage(damage);
        }
    }
}
