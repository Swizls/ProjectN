using TMPro;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitUI : MonoBehaviour
{
    private HealthBar _healthBar;

    private Unit _unit;

    private void Start()
    {
        _healthBar = GetComponentInChildren<HealthBar>();
        _unit = GetComponent<Unit>();
        _unit.Health.damageTaken += SetValues;
        SetValues();
    }
    private void OnEnable()
    {
        if(_unit != null)
        {
            _unit.Health.damageTaken += SetValues;
            SetValues();
        }
    }

    private void OnDisable()
    {
        _unit.Health.damageTaken -= SetValues;
    }

    private void SetValues()
    {
        if (_healthBar != null)
            _healthBar.SetScale(_unit.Health.HealthPoints);
    }
}
