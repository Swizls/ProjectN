using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnitControl : UnitControl
{
    public static EnemyUnitControl Instance;

    private void Awake()
    {
        Instance = this;

        _allControlableUnits = FindObjectsOfType<Unit>().Where(unit => unit.CompareTag("Enemy")).ToList();

        foreach (Unit unit in _allControlableUnits)
            unit.unitDied += OnUnitDeath;
    }

    private void OnEnable()
    {
        EndTurnHandler.turnEnd += OnEndTurn;
    }

    private void OnDisable()
    {
        EndTurnHandler.turnEnd -= OnEndTurn;
    }

    private void UnitControl()
    {
        for(int i = 0; i < _allControlableUnits.Count; i++)
        {
            _currentUnit = _allControlableUnits[i];
            TryAttack();
        }
    }

    private void OnEndTurn()
    { 
        UnitControl();
    }

    private bool TryAttack()
    {
        foreach (Unit playerUnit in PlayerUnitControl.Instance.AllControlableUnits)
        {
            if (_currentUnit.Actions.TryExecute(new ShootAtTargetAction(playerUnit)))
                return true;
        }
        return false;
    }

    public void SelectUnit(Unit unit)
    {
        _currentUnit = unit;
        _currentUnit.unitValuesUpdated.Invoke();
    }
}
