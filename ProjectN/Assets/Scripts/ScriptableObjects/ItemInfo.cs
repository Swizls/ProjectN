using UnityEngine;

[CreateAssetMenu(fileName = "Default Item", menuName ="Items/Item")]
public class ItemInfo : ScriptableObject, IItemInfo
{

    [SerializeField] private string _itemName = "Default Name";

    [SerializeField] private Sprite _sprite;

    [SerializeField] private IItemInfo.ItemType _type;

    [SerializeField] private float _weight;

    public IItemInfo.ItemType Type => _type;
    public Sprite Sprite => _sprite;
    public float Weight => _weight;
    public string Name => _itemName;

    public ItemInfo(IItemInfo.ItemType type, float weight)
    {
        _type = type;
        _weight = Mathf.Abs(weight);
    }

}
