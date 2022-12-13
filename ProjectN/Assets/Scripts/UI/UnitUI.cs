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
        _unit.unitValuesUpdated += SetValues;
        SetValues();
    }
    private void OnEnable()
    {
        if(_unit != null)
        {
            _unit.unitValuesUpdated += SetValues;
            SetValues();
        }
    }

    private void OnDisable()
    {
        _unit.unitValuesUpdated -= SetValues;
    }

    private void SetValues()
    {
        if(_healthBar != null)
            _healthBar.SetScale(_unit.Health.HealthPoints);
    }
}
