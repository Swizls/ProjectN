using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Weapon", menuName = "Items/Weapon")]
public class WeaponInfo : BaseItemInfo
{
    [SerializeField] private int _damage;
    [SerializeField] private int _magazineCapacity;

    [Range(0, 1)][SerializeField] private float _accuracy;

    public int Damage => _damage;
    public int MagazineCapacity => _magazineCapacity;
    public float Accuracy => _accuracy;
}
