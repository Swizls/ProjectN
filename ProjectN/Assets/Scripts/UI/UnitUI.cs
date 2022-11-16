using TMPro;
using UnityEngine;

[RequireComponent(typeof(UnitBehaviour))]
public class UnitUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI actionUnits;

    private UnitBehaviour unitBase;

    private void Start()
    {
        unitBase = GetComponent<UnitBehaviour>();
        unitBase.unitValuesUpdated += SetValues;
        SetValues();
    }

    private void OnDisable()
    {
        unitBase.unitValuesUpdated -= SetValues;
    }

    private void SetValues()
    {
        if(_healthText != null && _unit.Health != null)
            _healthText.text = _unit.Health.HealthPoints.ToString();
        if(_actionUnitsText != null)
            _actionUnitsText.text = _unit.Actions.ActionUnits.ToString();
    }
}
