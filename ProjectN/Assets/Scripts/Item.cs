using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemInfo info;

    private SpriteRenderer sprite;

    public ItemInfo Info => info;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sprite = info.Sprite;
    }
}
