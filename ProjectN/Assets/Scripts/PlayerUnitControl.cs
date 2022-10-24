using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerUnitControl : MonoBehaviour
{
    private Pathfinder pathFinder = new Pathfinder();

    private static List<UnitBase> allPlayerUnits;
    private static UnitBase currentSelectedUnit;

    private static Tilemap tileMap;

    public static List<UnitBase> AllPlayerUnits => allPlayerUnits;
    public static UnitBase CurrentSelectedUnit => currentSelectedUnit;

    private void Awake()
    {
        tileMap = FindObjectOfType<Tilemap>();

        allPlayerUnits = FindObjectsOfType<UnitBase>().Where(unit => unit.tag != "Enemy").ToList();
        currentSelectedUnit = allPlayerUnits[0];
    }

    private void Update()
    {
        UnitControl();
    }
    private void UnitControl()
    {
        //Rigth mouse button
        if (Input.GetMouseButtonDown(1) && !currentSelectedUnit.IsMoving && currentSelectedUnit.ActionUnits > 0)
        {
            List<Vector3> path = GetPath();
            currentSelectedUnit.SetPath(path);
        }
        //Left mouse button
        if (Input.GetMouseButtonDown(0) && !currentSelectedUnit.IsMoving)
        {
            if (!IsEnemeyInCell())
            {
                SelectUnit();
            }
            else
            {
                GameObject enemy = GetTarget();
                if (ObstacleCheckForShot(currentSelectedUnit.transform.position, enemy.transform.position))
                {
                    currentSelectedUnit.ShootAtTarget(enemy);
                }
            }
        }
    }

    public static void SelectUnit(UnitBase unit)
    {
        currentSelectedUnit = unit;
        currentSelectedUnit.unitValuesUpdated.Invoke();
    }
    private void SelectUnit()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if (hit.collider.tag == "PlayerUnit")
            {
                currentSelectedUnit = hit.collider.GetComponent<UnitBase>();
            }
        }
        currentSelectedUnit.unitValuesUpdated.Invoke();
    }
    private List<Vector3> GetPath()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<Vector3> path = pathFinder.FindPath(currentSelectedUnit.transform.position, clickPos, tileMap);

        return path;
    }
    private GameObject GetTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy")
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
    private bool IsEnemeyInCell()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy")
            {
                return true;
            }
        }
        return false;
    }
    private bool ObstacleCheckForShot(Vector3 startPosFloat, Vector3 targetPosFloat)
    {
        List<Vector3Int> pointsList = GetShotTrajectory(startPosFloat, targetPosFloat);

        foreach (Vector3Int point in pointsList)
        {
            RuleBaseTile tile = currentSelectedUnit.TileMap.GetTile<RuleBaseTile>(point);
            if (!tile.isPassable)
            {
                Debug.LogWarning("Obstacle check for a shot is: false! There is obstacle.");
                Debug.Log("Obstacle position: " + point);
                return false;
            }
        }
        return true;
    }
    private List<Vector3Int> GetShotTrajectory(Vector3 startPosFloat, Vector3 targetPosFloat)
    {
        List<Vector3Int> pointsList = new();

        Vector3Int startPosInt = tileMap.WorldToCell(startPosFloat);
        Vector3Int targetPosInt = tileMap.WorldToCell(targetPosFloat);

        Vector3 normalizedDireciton = (targetPosFloat - startPosFloat).normalized;
        Vector3Int roundedDirection = new((int)Mathf.Sign(normalizedDireciton.x), (int)Mathf.Sign(normalizedDireciton.y), 0);

        Vector3Int tileForCheck = startPosInt;

        while (tileForCheck.x != targetPosInt.x || tileForCheck.y != targetPosInt.y)
        {
            pointsList.Add(tileForCheck);
            if (tileForCheck.x != targetPosInt.x) tileForCheck.x += roundedDirection.x;
            if (tileForCheck.y != targetPosInt.y) tileForCheck.y += roundedDirection.y;
        }
        return pointsList;
    }
}