using TMPro;
using UnityEngine;

public class UnitStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI actionUnits;

    private void Start()
    {
        foreach(UnitBase unit in PlayerUnitHandler.AllPlayerUnits)
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
        health.text = PlayerUnitHandler.CurrentSelectedUnit.UnitHealth.ToString();
        actionUnits.text = PlayerUnitHandler.CurrentSelectedUnit.ActionUnits.ToString();
    }
}
