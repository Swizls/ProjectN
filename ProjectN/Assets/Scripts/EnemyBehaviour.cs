using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Unit _unit;

    private void Start()
    {
        _unit = GetComponent<Unit>();
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
        foreach(Unit playerUnit in PlayerUnitHandler.AllPlayerUnits)
        {
            if(_unit.Actions.TryExecute(new ShootAtTargetAction(playerUnit))) 
                return;
        }
    }
}