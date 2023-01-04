using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    private const int DEFAULT_HEALTH = 100;
    [SerializeField] private int _healthPoints;

    private ParticleSystem _particles;

    public Action healthValueChanged;

    public int HealthPoints => _healthPoints;

    private void Start()
    {
        _particles = GetComponentInChildren<ParticleSystem>();
    }

    public void Heal(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException();

        _healthPoints += value;

        if(_healthPoints > DEFAULT_HEALTH)
            _healthPoints = DEFAULT_HEALTH;

        healthValueChanged?.Invoke();
    }
    public void ApplyDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        _healthPoints -= damage;
        _particles.Play();

        healthValueChanged?.Invoke();

        if (_healthPoints <= 0)
            Death();
    }
    public void Death()
    {
        Destroy(gameObject);
    }
}
