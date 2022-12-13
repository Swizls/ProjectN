using UnityEngine;

public class Weapon
{
    private readonly int _damage;
    private readonly int _accuracy;
    
    private int _currentBulletCount;

    private readonly int _magazineMaxCapacity;

    public Weapon(WeaponInfo info)
    {
        _damage = info.Damage;
        _accuracy = info.Accuracy;
        _magazineMaxCapacity = info.MagazineCapacity;
        _currentBulletCount = info.MagazineCapacity;
    }

    public bool TryShoot(Unit target)
    {
        if(_currentBulletCount > 0)
        {
            target.Health.ApplyDamage(_damage);
            _currentBulletCount --;
            Debug.Log("Bullets count: " + _currentBulletCount);
            return true;
        }

        return false;
    }

    public void Reload()
    {
        _currentBulletCount = _magazineMaxCapacity;
        Debug.Log("Weapon reloaded. Bullets count: " + _currentBulletCount);
    }
}