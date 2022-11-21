using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storable armor", menuName = "Items/Armor/Storable armor")]
public class StorableArmorInfo : ArmorInfo, IStorableItemInfo
{
    [SerializeField] private float _maxCarringWeight;

    public float MaxCarringWeight => _maxCarringWeight;
}