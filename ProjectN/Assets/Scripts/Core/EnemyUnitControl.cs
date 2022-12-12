using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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
                        Move(FindFiringPosition(GetClosestTarget()));
                        yield return new WaitForSeconds(3);
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
        List<Vector3> pathToClosestTarget = _pathFinder.FindPath(_currentUnit.transform.position,
                                                                PlayerUnitControl.Instance.AllControlableUnits[0].transform.position,
                                                                _currentUnit.Tilemap);

        Unit target = PlayerUnitControl.Instance.AllControlableUnits[0];

        for (int i = 0; i < PlayerUnitControl.Instance.AllControlableUnits.Count; i++)
        {
            List<Vector3> pathToUnit = _pathFinder.FindPath(_currentUnit.transform.position,
                                                            PlayerUnitControl.Instance.AllControlableUnits[i].transform.position,
                                                            _currentUnit.Tilemap);

            if (pathToClosestTarget.Count > pathToUnit.Count)
            {
                pathToClosestTarget = pathToUnit;
                target = PlayerUnitControl.Instance.AllControlableUnits[i];
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
            if (ObstacleCheckForShot(path[i], target.transform.position, _currentUnit.Tilemap))
            {
                firingPoint = path[i];
                break;
            }
        }
        return firingPoint;
    }

    public bool ObstacleCheckForShot(Vector3 startPosFloat, Vector3 targetPosFloat, Tilemap tilemap)
    {
        foreach (Vector3Int point in GetShotTrajectory(startPosFloat, targetPosFloat, tilemap))
        {
            RuleBaseTile tile = tilemap.GetTile<RuleBaseTile>(point);
            if (!tile.isPassable)
            {
                Debug.LogWarning("Obstacle check for a shot is: false! There is obstacle. Obstacle position: " + point);
                return false;
            }
        }
        return true;
    }
    private List<Vector3Int> GetShotTrajectory(Vector3 startPos, Vector3 targetPos, Tilemap tilemap)
    {
        List<Vector3Int> pointsList = new();
        Vector3Int targetPosInt = tilemap.WorldToCell(targetPos);
        Vector3Int currentPointInt = tilemap.WorldToCell(startPos);

        pointsList.Add(currentPointInt);

        Vector3 direction = (targetPos - startPos).normalized;
        Vector3 nextpoint = currentPointInt;

        while (currentPointInt != targetPosInt)
        {
            if (currentPointInt.x != targetPosInt.x)
                nextpoint.x += direction.x;
            if (currentPointInt.y != targetPosInt.y)
                nextpoint.y += direction.y;

            currentPointInt = tilemap.WorldToCell(nextpoint);
            pointsList.Add(currentPointInt);
        }
        return pointsList;
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
