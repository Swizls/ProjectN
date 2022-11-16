using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private UnitBehaviour _unitBase;

    private void Start()
    {
        _unitBase = GetComponent<UnitBehaviour>();
    }

    private void OnEnable()
    {
        EndTurnHandler.turnEnd += OnEndTurn;
    }

    private void OnDisable()
    {
        EndTurnHandler.turnEnd -= OnEndTurn;
    }

    private void OnEndTurn()
    {
        foreach(UnitBehaviour unit in PlayerUnitHandler.AllPlayerUnits)
        {
            _unitBase.Actions.Execute(new ShootAtTargetAction(unit.gameObject));
            return;
        }
    }
}
