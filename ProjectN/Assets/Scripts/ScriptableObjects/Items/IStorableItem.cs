using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorableItem
{
    public float MaxCarringWeight { get; }
    public float CurrentCarringWeight { get; }
    public List<BaseItemInfo> StoredItems { get; }

    public void TryToAddItem(BaseItemInfo item);
    public void TryToTransitItem(BaseItemInfo item, IStorableItem container);
    public void RemoveItem(BaseItemInfo item);
}
