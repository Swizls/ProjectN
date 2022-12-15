using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCrate : MonoBehaviour
{
    [SerializeField] private List<BaseItemInfo> _items = new List<BaseItemInfo>();

    public List<BaseItemInfo> Items => _items;

    private void RemoveItem(BaseItemInfo item)
    {
        _items.Remove(item);
    }
}
