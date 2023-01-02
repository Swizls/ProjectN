using UnityEngine;

public class ArmorUI : MonoBehaviour, IInventoryUI
{
    public bool TryInteract(BaseItemInfo item, IInventoryUI to)
    {
        return TryTransitItem(item, to);
    }

    private bool TryTransitItem(BaseItemInfo item, IInventoryUI to)
    {
        var unitInventory = PlayerUnitControl.Instance.CurrentUnit.Inventory;
        if (to is BackpackUI)
        {
            return unitInventory.Armor.TryToTransit(item, unitInventory.Backpack);
        }
        else if (to is ExternalUI)
        {
            unitInventory.Armor.Remove(item);
            return true;
        }
        return false;
    }
}