using UnityEngine;

public class BackpackUI : MonoBehaviour, IInventoryUI
{
    public bool TryInteract(BaseItemInfo item, IInventoryUI to)
    {
        return TryTransitItem(item, to);
    }

    private bool TryTransitItem(BaseItemInfo item, IInventoryUI to)
    {
        var unitInventory = PlayerUnitControl.Instance.CurrentUnit.Inventory;
        if (to is ArmorUI)
        {
            return unitInventory.Backpack.TryToTransit(item, unitInventory.Armor);
        }
        else if (to is ExternalUI)
        {
            unitInventory.Backpack.Remove(item);
            return true;
        }
        return false;
    }
}
