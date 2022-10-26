using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class PlayerUnitHandler : MonoBehaviour
{
    private const float PICKABLE_RANGE = 1.5f;

    private Pathfinder pathFinder = new Pathfinder();

    private static List<UnitBase> allPlayerUnits;
    private static UnitBase currentSelectedUnit;

    private static Tilemap tileMap;
    private static LineRenderer lineRenderer;

    public static List<UnitBase> AllPlayerUnits => allPlayerUnits;
    public static UnitBase CurrentSelectedUnit => currentSelectedUnit;

    private void Awake()
    {
        tileMap = FindObjectOfType<Tilemap>();
        lineRenderer = GetComponent<LineRenderer>();

        allPlayerUnits = FindObjectsOfType<UnitBase>().Where(unit => unit.tag != "Enemy").ToList();
        currentSelectedUnit = allPlayerUnits[0];
    }

    private void Update()
    {
        ShowPath();
        UnitControl();
    }

    private void ShowPath()
    {
        Vector3[] path = GetPath()?.ToArray();
        if(path != null)
        {
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = new Vector3(path[i].x, path[i].y, -1);
            }

            lineRenderer.positionCount = path.Length;
            lineRenderer.SetPositions(path);

            if (currentSelectedUnit.ActionUnits >= path.Length * UnitBase.MOVE_COST)
            {
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
            }
            else
            {
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
    private void UnitControl()
    {
        //Left mouse button
        if (Input.GetMouseButtonDown(0) && !currentSelectedUnit.IsMoving)
        { 
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                                 Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(hit.collider != null)
            {
                if (IsUnit(hit))
                {
                    SelectUnit(hit);
                }
                else if (IsEnemey(hit) && currentSelectedUnit.ActionUnits >= UnitBase.SHOT_COST)
                {
                    GameObject enemy = GetTarget();
                    if (ObstacleCheckForShot(currentSelectedUnit.transform.position, enemy.transform.position))
                    {
                        currentSelectedUnit.ShootAtTarget(enemy);
                    }
                }
                else if (IsItem(hit) && currentSelectedUnit.ActionUnits >= UnitBase.INTERACTION_COST)
                {
                    if (Vector3.Distance(currentSelectedUnit.transform.position, hit.collider.transform.position) < PICKABLE_RANGE)
                    {
                        currentSelectedUnit.PickupItem(hit.collider.gameObject);
                    }
                }
            }    
        }
        //Rigth mouse button
        if (Input.GetMouseButtonDown(1) && !currentSelectedUnit.IsMoving && currentSelectedUnit.ActionUnits >= GetPath().Count * UnitBase.MOVE_COST)
        {
            List<Vector3> path = GetPath();
            currentSelectedUnit.StartMove(path);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentSelectedUnit.Inventory.AddItem(new ItemInfo(ItemInfo.ItemType.weapon, 10f));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurnHandler.EndTurn();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private bool IsUnit(RaycastHit2D hit)
    {
        if (hit.collider.tag == "PlayerUnit") 
            return true;
        return false;
    }

    private bool IsItem(RaycastHit2D hit)
    {
        if (hit.collider.gameObject.GetComponent<Item>() != null)
            return true;
        return false;
    }
    private bool IsEnemey(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Enemy")
            return true;
        return false;
    }
    public static void SelectUnit(UnitBase unit)
    {
        currentSelectedUnit = unit;
        currentSelectedUnit.unitValuesUpdated.Invoke();
    }
    private void SelectUnit(RaycastHit2D hit)
    {
        if (hit.collider.tag == "PlayerUnit")
            currentSelectedUnit = hit.collider.GetComponent<UnitBase>();
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