using UnityEngine;

public class ArmorInfo : BaseItemInfo
{
    [SerializeField] private int _armorPoitns;

    public int ArmorPoints => _armorPoitns;
}
