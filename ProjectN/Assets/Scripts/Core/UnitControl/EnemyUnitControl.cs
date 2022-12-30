using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ShotUtilites;

public class EnemyUnitControl : UnitControl
{
    private const int MOVE_COST = 2;

    private readonly Pathfinder _pathFinder = new Pathfinder();

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
        for (int i = 0; i < _allControlableUnits.Count; i++)
        {
            _currentUnit = _allControlableUnits[i];
            while (_currentUnit.Actions.ActionUnits >= MOVE_COST)
            {
                if (PlayerUnitControl.Instance.AllControlableUnits.Count > 0)
                {
                    if (TryAttack())
                    {
                        yield return new WaitForSeconds(1);
                    }
                    else
                    {
                        Unit closestTarget = GetClosestTarget();
                        if(closestTarget != null)
                        {
                            Move(FindFiringPosition(closestTarget));
                            yield return new WaitForSeconds(3);
                        }
                        else
                        {
                            yield return new WaitForSeconds(1);
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
        EndTurnHandler.EndTurn();
    }

    private Unit GetClosestTarget()
    {
        Unit target = null;
        List<Vector3> lastPathToUnit = new();

        for (int i = 0; i < PlayerUnitControl.Instance.AllControlableUnits.Count; i++)
        {
            List<Vector3> pathToUnit = _pathFinder.FindPath(_currentUnit.transform.position,
                                                            PlayerUnitControl.Instance.AllControlableUnits[i].transform.position,
                                                            _currentUnit.Tilemap);
            if(pathToUnit != null)
            {
                if (lastPathToUnit.Count < pathToUnit.Count)
                {
                    lastPathToUnit = pathToUnit;
                    target = PlayerUnitControl.Instance.AllControlableUnits[i];
                }
            }
        }
        return target;
    }

    private void Move(Vector3 targetPosition)
    {
        _currentUnit.Actions.TryExecute(new MoveAction(targetPosition));
    }
    
    private Vector3 FindFiringPosition(Unit target)
    {
        List<Vector3> path = _pathFinder.FindPath(_currentUnit.transform.position,
                                                  target.transform.position, _currentUnit.Tilemap);
        Vector3 firingPoint = path.Last();

        for (int i = 0; i < path.Count; i++)
        {
            if (ShotUtilities.ObstacleCheckForShot(path[i], target.transform.position, _currentUnit.Tilemap))
            {
                firingPoint = path[i];
                break;
            }
        }
        return firingPoint;
    }

    private void OnTurnStart()
    {
        StartCoroutine(UnitControl());
    }

    private bool TryAttack()
    {
        foreach (Unit playerUnit in PlayerUnitControl.Instance.AllControlableUnits)
        {
            if (_currentUnit.Actions.TryExecute(new ShootAction(playerUnit)))
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
