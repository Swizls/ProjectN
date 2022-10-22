using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerUnitControl : MonoBehaviour
{
    private static Pathfinder pathFinder = new Pathfinder();

    private static List<UnitBase> allPlayerUnits;
    private static UnitBase selectedUnit;
    private static Tilemap tileMap;

    public static List<UnitBase> AllPlayerUnits => allPlayerUnits;

    private void Start()
    {
        tileMap = FindObjectOfType<Tilemap>();

        allPlayerUnits = FindObjectsOfType<UnitBase>().Where(unit => unit.tag != "Enemy").ToList();
        selectedUnit = allPlayerUnits[0];
    }

    private void Update()
    {
        UnitControl();
    }

    private void UnitControl()
    {
        //Rigth mouse button
        if (Input.GetMouseButtonDown(1) && !selectedUnit.IsMoving)
        {
            List<Vector3> path = GetPath();
            selectedUnit.SetPath(path);
        }
        //Left mouse button
        if (Input.GetMouseButtonDown(0) && !selectedUnit.IsMoving)
        {
            if (!IsEnemeyInCell())
            {
                SelectUnit();
            }
            else
            {
                GameObject enemy = GetTarget();
                if (ObstacleCheckForShot(selectedUnit.transform.position, enemy.transform.position))
                {
                    selectedUnit.ShootAtTarget(enemy);
                }
            }
        }
    }

    public static void SelectUnit(UnitBase unit)
    {
        selectedUnit = unit;
    }
    private void SelectUnit()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                                Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if (hit.collider.tag == "PlayerUnit")
            {
                selectedUnit = hit.collider.GetComponent<UnitBase>();
            }
        }
    }
    private List<Vector3> GetPath()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<Vector3> path = pathFinder.FindPath(selectedUnit.transform.position, clickPos, selectedUnit.TileMap);

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
            RuleBaseTile tile = selectedUnit.TileMap.GetTile<RuleBaseTile>(point);
            if (!tile.isPassable)
            {
                Debug.LogWarning("There is obstacle!");
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

        Vector3Int roundedDirection = tileMap.WorldToCell(new Vector3(Mathf.Round(normalizedDireciton.x), 
                                                                      Mathf.Round(normalizedDireciton.y),
                                                                      normalizedDireciton.z));

        Vector3Int tileForCheck = startPosInt;

        while ((tileForCheck.x != targetPosInt.x || tileForCheck.y != targetPosInt.y) && pointsList.Count < 1000)
        {
            pointsList.Add(tileForCheck);
            if (tileForCheck.x != targetPosInt.x) tileForCheck.x += roundedDirection.x;
            if (tileForCheck.y != targetPosInt.y) tileForCheck.y += roundedDirection.y;
        }
        return pointsList;
    }
}