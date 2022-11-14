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
        //_itemsInBackpack = PlayerUnitHandler.CurrentSelectedUnit.Inventory.ItemsInBackpack;
        //_itemsInArmor = PlayerUnitHandler.CurrentSelectedUnit.Inventory.ItemsInArmor;
        _externalItems = PlayerUnitHandler.CurrentSelectedUnit.Inventory.GetItemsOnGround().ToList();

        _allItemsInInventory.Clear();
        _allItemsInInventory.Concat(_itemsInBackpack);
        _allItemsInInventory.Concat(_itemsInArmor);
    }
    private void Render()
    {
        UpdateItemLists();

        RenderBackpack();
        RenderArmor();
        RenderExternal();
    }

    private void RenderArmor()
    {
        for (int i = 0; i < _itemsInArmor.Count; i++)
        {
            GameObject createdItem = Instantiate(_inventoryItemPrefab, _armorAreaUI);
            createdItem.name = _itemsInArmor[i].Name;
            createdItem.GetComponent<ItemInventoryPresenter>()._itemInfo = _itemsInArmor[i];
            createdItem.GetComponent<Image>().sprite = _itemsInArmor[i].Sprite;
            _renderedItems.Add(createdItem);
        }
    }

    private void RenderBackpack()
    {
        for (int i = 0; i < _itemsInBackpack.Count; i++)
        {
            GameObject createdItem = Instantiate(_inventoryItemPrefab, _backpackAreaUI);
            createdItem.name = _itemsInBackpack[i].Name;
            createdItem.GetComponent<ItemInventoryPresenter>()._itemInfo = _itemsInBackpack[i];
            createdItem.GetComponent<Image>().sprite = _itemsInBackpack[i].Sprite;
            _renderedItems.Add(createdItem);
        }
    }
    private void RenderExternal()
    {
        for (int i = 0; i < _externalItems.Count; i++)
        {
            GameObject createdItem = Instantiate(_inventoryItemPrefab, _externalAreaUI);
            createdItem.name = _externalItems[i].Name;
            createdItem.GetComponent<ItemInventoryPresenter>()._itemInfo = _externalItems[i];
            createdItem.GetComponent<Image>().sprite = _externalItems[i].Sprite;
            _renderedItems.Add(createdItem);
        }
    }
}
