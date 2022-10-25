using TMPro;
using UnityEngine;

public class UnitStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI actionUnits;

    private void Start()
    {
        foreach(UnitBase unit in PlayerUnitControl.AllPlayerUnits)
        {
            unit.unitValuesUpdated += SetValues;
        }
        SetValues();
    }

    private void OnDisable()
    {
        PlayerUnitControl.CurrentSelectedUnit.unitValuesUpdated -= SetValues;
    }

    private void SetValues()
    {
        health.text = PlayerUnitControl.CurrentSelectedUnit.UnitHealth.ToString();
        actionUnits.text = PlayerUnitControl.CurrentSelectedUnit.ActionUnits.ToString();
    }
}
