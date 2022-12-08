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
        EndTurnHandler.enemyTurn += OnTurnStart;
    }

    private void OnDisable()
    {
        EndTurnHandler.enemyTurn -= OnTurnStart;
    }

    private IEnumerator UnitControl()
    {
        for(int i = 0; i < _allControlableUnits.Count; i++)
        {
            _currentUnit = _allControlableUnits[i];
            if (TryAttack())
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                Move();
                yield return new WaitForSeconds(3);
            }
        }
        EndTurnHandler.EndTurn();
    }

    private void Move()
    {
        _currentUnit.Actions.TryExecute(new MoveAction(PlayerUnitControl.Instance.CurrentSelectedUnit.transform.position));
    }

    private void OnTurnStart()
    {
        StartCoroutine(UnitControl());
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
