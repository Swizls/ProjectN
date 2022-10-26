using UnityEngine;

[CreateAssetMenu(fileName = "Default Item", menuName ="Items/Item")]
public class ItemInfo : ScriptableObject
{
    public enum ItemType
    {
        undefined,
        weapon,
        food
    }

    [SerializeField] private ItemType itemType;

    [SerializeField] private Sprite sprite;

    [SerializeField] private float weight;

    [SerializeField] private bool isWeapon;

    [SerializeField] private string itemName = "Default Name";

    public ItemType ItemType1 => itemType;
    public Sprite Sprite => sprite;
    public float Weight => weight;
    public bool IsWeapon => isWeapon;
    public string ItemName => itemName;

    public ItemInfo()
    {

    }
    public ItemInfo(ItemType type, float weight)
    {
        itemType = type;
        this.weight = Mathf.Abs(weight);
    }

}
