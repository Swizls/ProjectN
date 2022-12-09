using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Weapon", menuName = "Items/Weapon")]
public class WeaponInfo : BaseItemInfo
{
    [SerializeField] private int _damage;
    [SerializeField] private int _magazineCapacity;
    [SerializeField] private int _accuracy;

    public int Damage => _damage;
    public int MagazineCapacity => _magazineCapacity;
    public int Accuracy => _accuracy;
}
