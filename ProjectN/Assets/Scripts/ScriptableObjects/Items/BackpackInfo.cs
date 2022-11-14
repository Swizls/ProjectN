using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Backpack", menuName = "Items/Backpack")]
public class BackpackInfo : BaseItemInfo
{
    [SerializeField] private int _maxCarringWeight;

    public int MaxCarringWeight => _maxCarringWeight;
}
