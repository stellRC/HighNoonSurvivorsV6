using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int damage = 1;
    public List<IDoDamage> damageTypes;
    public IDoDamage damageType;

    public void TryDoAttack()
    {
        // Do damage on each item in list
        foreach (IDoDamage damage in damageTypes)
        {
            damage.DoDamage(this.damage);
        }
    }

    // Change behavior at runtime
    public void SetDamageType(IDoDamage damageType)
    {
        this.damageType = damageType;
    }
}
