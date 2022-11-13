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
        if(health != null)
            health.text = unitBase.UnitHealth.ToString();
        if(actionUnits != null)
            actionUnits.text = unitBase.ActionUnits.ToString();
    }
}
