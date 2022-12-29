using TMPro;
using UnityEngine;

public class UnitStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI actionUnits;

    private void Start()
    {
        foreach(Unit unit in PlayerUnitControl.Instance.AllControlableUnits)
        {
            unit.unitValuesUpdated += SetValues;
        }
        SetValues();
    }

    private void OnDisable()
    {
        PlayerUnitControl.Instance.CurrentUnit.unitValuesUpdated -= SetValues;
    }

    private void SetValues()
    {
        health.text = PlayerUnitControl.Instance.CurrentUnit.Health.HealthPoints.ToString();
        actionUnits.text = PlayerUnitControl.Instance.CurrentUnit.Actions.ActionUnits.ToString();
    }
}
