using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemInfo
{
    public enum ItemType
    {
        undefined,
        weapon,
        food,
        medicine,
        backpack,
        armor
    }

    public ItemType Type { get; }
    public Sprite Sprite { get; }
    public float Weight { get; }
    public string Name { get; }
}
