using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Weapon", menuName = "Items/Weapon")]
public class WeaponInfo : BaseItemInfo
{
    [SerializeField] private int _weaponBaseDamage;
    [SerializeField] private int _weaponMagazineCapacity;

    public int WeaponBaseDamage => _weaponBaseDamage;
    public int WeaponMagazineCapacity => _weaponMagazineCapacity;
}
