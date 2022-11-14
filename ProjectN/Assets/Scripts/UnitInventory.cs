using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    private float _currentCarringWeightInBackpack = 0f;
    private float _currentCarringWeightInArmor = 0f;

    private List<BaseItemInfo> _itemsInBackpack = new();
    private List<BaseItemInfo> _itemsInArmor = new();

    [SerializeField] private BackpackInfo _currentBackpack;
    [SerializeField] private BackpackInfo _currentArmor;
    private WeaponInfo _currentWeapon;

    public Action<BaseItemInfo> newItemAdded;

    public BaseItemInfo CurrentBackpack => _currentBackpack;
    public WeaponInfo CurrentWeapon => _currentWeapon;
    public List<BaseItemInfo> ItemsInBackpack => _itemsInBackpack;
    public List<BaseItemInfo> ItemsInArmor => _itemsInArmor;

    public void AddItem(BaseItemInfo item)
    {
        if(_currentCarringWeightInBackpack + item.Weight <= _currentBackpack.MaxCarringWeight)
        {
            _itemsInBackpack.Add(item);
            _currentCarringWeightInBackpack += item.Weight;
        }
    }
    public void RemoveItem(BaseItemInfo item)
    {
        if(_itemsInBackpack.Contains(item))
        {
            _itemsInBackpack.Remove(item);
            _currentCarringWeightInBackpack -= item.Weight;
        }
        else
        {
            _itemsInArmor.Remove(item);
            _currentCarringWeightInBackpack -= item.Weight;
        }
    }

    public bool TryToTransitItem(BaseItemInfo item, bool toBackpack)
    {
        if (toBackpack)
        {
            if(_currentCarringWeightInBackpack + item.Weight <= _currentBackpack.MaxCarringWeight)
            {
                _itemsInArmor.Remove(item);
                _itemsInBackpack.Add(item);
                _currentCarringWeightInBackpack = CalculateCurrentCarringWeight(_itemsInBackpack);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if(_currentCarringWeightInArmor + item.Weight <= _currentArmor.MaxCarringWeight)
            {
                _itemsInBackpack.Remove(item);
                _itemsInArmor.Add(item);
                _currentCarringWeightInArmor = CalculateCurrentCarringWeight(_itemsInArmor);
                return true;
            }
        }
        return false;
    }

    private float CalculateCurrentCarringWeight(List<BaseItemInfo> items)
    {
        float currentWieght = 0;
        foreach(var item in items)
        {
            currentWieght += item.Weight;
        }
        return currentWieght;
    }
}
