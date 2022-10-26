using UnityEngine;

[CreateAssetMenu(fileName = "Default Item", menuName ="Items/Item")]
public class Item : ScriptableObject
{
    private enum ItemType
    {
        undefined,
        weapon,
        food
    }

    [SerializeField] private ItemType itemType;

    [SerializeField] private Sprite icon;

    [SerializeField] private uint itemWheight;

    [SerializeField] private bool isWeapon;

    [SerializeField] private string itemName = "Default Name";

    private ItemType ItemType1 { get => itemType; set => itemType = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public uint ItemWheight { get => itemWheight; set => itemWheight = value; }
    public bool IsWeapon { get => isWeapon; set => isWeapon = value; }
    public string ItemName { get => itemName; set => itemName = value; }
}
