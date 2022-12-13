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
            if(_accuracy >= Random.Range(0, 100))
            { 
                target.Health.ApplyDamage(_damage);
            }
            Debug.Log("Bullets count: " + _currentBulletCount);
            _currentBulletCount --;
            return true;
        }

        return false;
    }

    public bool TryReload()
    {
        if (_currentBulletCount == _magazineMaxCapacity)
            return false;

        _currentBulletCount = _magazineMaxCapacity;
        Debug.Log("Weapon reloaded. Bullets count: " + _currentBulletCount);

        return true;
    }
}