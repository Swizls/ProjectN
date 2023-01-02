using UnityEngine;

public interface IInventoryUI
{
    public bool TryInteract(BaseItemInfo item, IInventoryUI to = null);
}
