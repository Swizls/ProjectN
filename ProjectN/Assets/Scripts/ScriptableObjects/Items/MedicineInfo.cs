using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Medicine", menuName = "Items/Medicine")]
public class MedicineInfo : BaseItemInfo
{
    [SerializeField] private int _healPoints;

    public int HealPoints => _healPoints;
}
