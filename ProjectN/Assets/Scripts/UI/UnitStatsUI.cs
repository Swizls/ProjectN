using TMPro;
using UnityEngine;

public class UnitStatsUI : MonoBehaviour
{
    [SerializeField] private HealthBar _health;
    [SerializeField] private TextMeshProUGUI _actionUnits;

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
        _health.SetScale(PlayerUnitControl.Instance.CurrentUnit.Health.HealthPoints);
        _actionUnits.text = PlayerUnitControl.Instance.CurrentUnit.Actions.ActionUnits.ToString();
    }
}
