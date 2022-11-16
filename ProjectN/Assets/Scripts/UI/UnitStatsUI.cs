using TMPro;
using UnityEngine;

public class UnitStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI actionUnits;

    private void Start()
    {
        foreach(UnitBehaviour unit in PlayerUnitHandler.AllPlayerUnits)
        {
            unit.unitValuesUpdated += SetValues;
        }
        SetValues();
    }

    private void OnDisable()
    {
        PlayerUnitHandler.CurrentSelectedUnit.unitValuesUpdated -= SetValues;
    }

    private void SetValues()
    {
        health.text = PlayerUnitHandler.CurrentSelectedUnit.Health.HealthPoints.ToString();
        actionUnits.text = PlayerUnitHandler.CurrentSelectedUnit.Actions.ActionUnits.ToString();
    }
}
