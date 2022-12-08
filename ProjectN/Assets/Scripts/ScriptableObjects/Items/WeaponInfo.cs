using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Weapon", menuName = "Items/Weapon")]
public class WeaponInfo : BaseItemInfo
{
    [SerializeField] private int _weaponDamage;
    [SerializeField] private int _weaponMagazineCapacity;
    [SerializeField] private int _weaponAccuracy;

    public int WeaponDamage => _weaponDamage;
    public int WeaponMagazineCapacity => _weaponMagazineCapacity;
    public int WeaponAccuracy => _weaponAccuracy;
}
