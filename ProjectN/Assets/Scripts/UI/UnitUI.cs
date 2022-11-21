using TMPro;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _actionUnitsText;

    private Unit _unit;

    private void Start()
    {
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
        if (_healthText != null && _unit.Health != null)
            _healthText.text = _unit.Health.HealthPoints.ToString();
        if (_actionUnitsText != null && _unit.Actions != null)
            _actionUnitsText.text = _unit.Actions.ActionUnits.ToString();
    }
}
