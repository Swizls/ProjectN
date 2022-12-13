using UnityEngine;

public class Weapon
{
    private const float ACCURACY_REDUCE_PER_TILE = 0.02f;
    private const int DISTANCE_WITHOUT_ACCURACY_DEBUFF = 5;

    private readonly int _damage;

    private readonly float _accuracy;
    
    private readonly int _magazineMaxCapacity;

    private int _currentBulletCount;

    public Weapon(WeaponInfo info)
    {
        _damage = info.Damage;
        _accuracy = info.Accuracy;
        _magazineMaxCapacity = info.MagazineCapacity;
        _currentBulletCount = info.MagazineCapacity;
    }

    public bool TryShoot(Unit target, int distanceToTarget)
    {
        float finalHitChance = _accuracy;
        if(distanceToTarget > DISTANCE_WITHOUT_ACCURACY_DEBUFF)
        {
            finalHitChance = _accuracy - ((distanceToTarget - DISTANCE_WITHOUT_ACCURACY_DEBUFF) * ACCURACY_REDUCE_PER_TILE);
        }
        if(finalHitChance <= 0)
        {
            finalHitChance = 0.01f;
        }
        if(_currentBulletCount > 0)
        {
            if(finalHitChance >= Random.Range(0, 1f))
            { 
                target.Health.ApplyDamage(_damage);
            }
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
        return true;
    }
}