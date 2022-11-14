using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryItemPrefab;

    [SerializeField] private Transform _backpackAreaUI;
    [SerializeField] private Transform _armorAreaUI;
    [SerializeField] private Transform _externalAreaUI;
    [SerializeField] private Transform _currentWeaponUI;

    private List<BaseItemInfo> _itemsInArmor = new();
    private List<BaseItemInfo> _itemsInBackpack = new();
    private List<BaseItemInfo> _externalItems = new();

    private List<BaseItemInfo> _allItemsInInventory = new();

    private List<GameObject> _renderedItems = new();

    public List<BaseItemInfo> AllItemsInInventory => _allItemsInInventory;

    public Transform BackpackAreaUI => _backpackAreaUI;
    public Transform ArmorAreaUI => _armorAreaUI;
    public Transform ExternalAreaUI => _externalAreaUI;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Render();
    }
    private void OnDisable()
    {
        if (_renderedItems.Count > 0)
        {
            foreach (GameObject item in _renderedItems)
            {
                Destroy(item);
            }
            _renderedItems.Clear();
        }
    }
    private void UpdateItemLists()
    {
        _itemsInBackpack = PlayerUnitHandler.CurrentSelectedUnit.Inventory.Backpack.StoredItems;
        _itemsInArmor = PlayerUnitHandler.CurrentSelectedUnit.Inventory.Armor.StoredItems;
        _externalItems = PlayerUnitHandler.CurrentSelectedUnit.Inventory.GetItemsOnGround().ToList();

        _allItemsInInventory.Clear();
        _allItemsInInventory.Concat(_itemsInBackpack);
        _allItemsInInventory.Concat(_itemsInArmor);
    }
    private void Render()
    {
        UpdateItemLists();

        RenderItem(_armorAreaUI, _itemsInArmor);
        RenderItem(_backpackAreaUI, _itemsInBackpack);
        RenderItem(_externalAreaUI, _externalItems);
    }

    private void RenderItem(Transform area, List<BaseItemInfo> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject createdItem = Instantiate(_inventoryItemPrefab, area);
            createdItem.name = items[i].Name;
            createdItem.GetComponent<ItemInventoryPresenter>()._itemInfo = items[i];
            createdItem.GetComponent<Image>().sprite = items[i].Sprite;
            _renderedItems.Add(createdItem);
        }
    }
}
