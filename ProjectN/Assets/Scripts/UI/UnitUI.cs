using TMPro;
using UnityEngine;

[RequireComponent(typeof(UnitBase))]
public class UnitUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI actionUnits;

    private UnitBase unitBase;

    private void Start()
    {
        unitBase = GetComponent<UnitBase>();
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
