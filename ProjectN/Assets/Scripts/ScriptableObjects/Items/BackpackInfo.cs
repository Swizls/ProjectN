using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Backpack", menuName = "Items/Backpack")]
public class BackpackInfo : BaseItemInfo, IStorableItemInfo
{
    [SerializeField] private float _maxCarringWeight;

    public float MaxCarringWeight => _maxCarringWeight;
}
