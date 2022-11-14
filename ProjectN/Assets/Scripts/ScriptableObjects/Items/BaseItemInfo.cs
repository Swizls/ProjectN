using UnityEngine;

[CreateAssetMenu(fileName = "Default Item", menuName ="Items/Item")]
public class BaseItemInfo : ScriptableObject
{
    [SerializeField] private string _itemName = "Default Name";
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _weight;

    public Sprite Sprite => _sprite;
    public float Weight => _weight;
    public string Name => _itemName;
}
