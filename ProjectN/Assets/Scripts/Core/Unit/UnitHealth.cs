using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] private int _healthPoints;

    private ParticleSystem _particles;

    public Action damageTaken;

    public int HealthPoints => _healthPoints;

    private void Start()
    {
        _particles = GetComponentInChildren<ParticleSystem>();
    }

    public void ApplyDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        _healthPoints -= damage;
        _particles.Play();

        damageTaken?.Invoke();

        if (_healthPoints <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        Destroy(gameObject);
    }
}
