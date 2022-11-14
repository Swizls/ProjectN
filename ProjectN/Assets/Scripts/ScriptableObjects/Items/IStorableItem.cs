using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorableItem
{
    public float MaxCarringWeight { get; }
    public float CurrentCarringWeight { get; }
    public List<BaseItemInfo> StoredItems { get; }

    public bool TryToAddItem(BaseItemInfo item);
    public void RemoveItem(BaseItemInfo item);
}
