using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerUnitControl : UnitBase
{
    Pathfinder pathFinder = new Pathfinder();
    PathfinderDebug pathfinderDebug = new PathfinderDebug();
    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {
        UnitControl();
    }
    protected void UnitControl()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Vector3Int cellPosition = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            pathList = pathFinder.FindPath(unitPos, cellPosition, tileMap);
            isMoving = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int cellPosition = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            StartCoroutine(pathfinderDebug.FindPath(unitPos, cellPosition, tileMap));
        }
    }
}
