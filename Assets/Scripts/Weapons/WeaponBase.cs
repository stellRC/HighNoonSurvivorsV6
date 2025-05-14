using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int Damage = 1;
    public List<IDoDamage> DamageTypes;
    public IDoDamage DamageType;

    public void TryDoAttack()
    {
        // Do damage on each item in list
        foreach (IDoDamage damage in DamageTypes)
        {
            damage.DoDamage(this.Damage);
        }
    }

    // Change behavior at runtime
    public void SetDamageType(IDoDamage damageType)
    {
        this.DamageType = damageType;
    }
}
