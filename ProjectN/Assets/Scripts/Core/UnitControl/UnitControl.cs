using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitControl : MonoBehaviour
{
    protected List<Unit> _allControlableUnits;
    protected Unit _currentUnit;

    public List<Unit> AllControlableUnits => _allControlableUnits;
    public Unit CurrentUnit => _currentUnit;

    protected void OnUnitDeath(Unit unit)
    {
        _allControlableUnits.Remove(unit);
        unit.unitDied -= OnUnitDeath;
    }
}
