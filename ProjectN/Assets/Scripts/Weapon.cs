using UnityEngine;

public class Weapon
{
    private const float ACCURACY_REDUCE_PER_TILE = 0.02f;
    private const int DISTANCE_WITHOUT_ACCURACY_DEBUFF = 5;
    private const float MIN_HIT_CHANCE = 0.1f;
    private const float MAX_ACCURACY_REDUCE_FROM_COVERS = 0.9f;

    private readonly int _damage;
    private readonly float _accuracy;
    
    private readonly int _magazineMaxCapacity;
    private int _currentBulletCount;

    private readonly Sprite _sprite;

    public Sprite Sprite => _sprite;
    public int MagazineMaxCapacity => _magazineMaxCapacity;
    public int CurrentBulletCount => _currentBulletCount;

    public Weapon(WeaponInfo info)
    {
        _damage = info.Damage;
        _accuracy = info.Accuracy;
        _magazineMaxCapacity = info.MagazineCapacity;
        _currentBulletCount = info.MagazineCapacity;
        _sprite = info.Sprite;
    }

    public bool TryShoot(Unit target, int distanceToTarget, Cover[] obstacles)
    {
        float finalHitChance;

        if(distanceToTarget > DISTANCE_WITHOUT_ACCURACY_DEBUFF)
        {
            finalHitChance = _accuracy - ((distanceToTarget - DISTANCE_WITHOUT_ACCURACY_DEBUFF) * ACCURACY_REDUCE_PER_TILE);
        }
        else
        {
            finalHitChance = _accuracy;
        }

        finalHitChance -= SumObstaclesAccuracyReduce(obstacles);

        if (finalHitChance <= 0)
        {
            finalHitChance = MIN_HIT_CHANCE;
        }
        Debug.Log(finalHitChance);
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

    private float SumObstaclesAccuracyReduce(Cover[] obstacles)
    {
        float result = 0f;
        foreach (Cover obstacle in obstacles)
        {
            result += obstacle.AccuracyReduce;
        }
        if (result > MAX_ACCURACY_REDUCE_FROM_COVERS)
        {
            result = MAX_ACCURACY_REDUCE_FROM_COVERS;
        }
        return result;
    }

    public bool TryReload()
    {
        if (_currentBulletCount == _magazineMaxCapacity)
            return false;

        _currentBulletCount = _magazineMaxCapacity;
        return true;
    }
}