using UnityEngine;

public class ExternalUI : MonoBehaviour, IInventoryUI
{
    public bool TryInteract(BaseItemInfo item, IInventoryUI to)
    {
        return TryTransitItem(item, to);
    }
    private bool TryTransitItem(BaseItemInfo item, IInventoryUI to)
    {
        var unitInventory = PlayerUnitControl.Instance.CurrentUnit.Inventory;
        if (to is BackpackUI)
            return unitInventory.Backpack.TryToAdd(item);
        else if (to is ArmorUI)
            return unitInventory.Armor.TryToAdd(item);
        return false;
    }
}
