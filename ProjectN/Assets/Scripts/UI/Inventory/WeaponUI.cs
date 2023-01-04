using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour, IInventoryUI
{
    [SerializeField] private Image _sprite;
    [SerializeField] private TextMeshProUGUI _ammoCount;

    private void OnEnable()
    {
        var unitInventroy = PlayerUnitControl.Instance.CurrentUnit.Inventory;
        _sprite.sprite = unitInventroy.Weapon.Sprite;
        _ammoCount.text = $"{unitInventroy.Weapon.CurrentBulletCount}/{unitInventroy.Weapon.MagazineMaxCapacity}";
    }
    public bool TryInteract(BaseItemInfo item, IInventoryUI to = null)
    {
        return TryEquipWeapon((WeaponInfo)item);
    }
    private bool TryEquipWeapon(WeaponInfo weapon)
    {
        var unitInventory = PlayerUnitControl.Instance.CurrentUnit.Inventory;
        unitInventory.Weapon = new(weapon);
        _sprite.sprite = weapon.Sprite;
        return true;
    }
}
