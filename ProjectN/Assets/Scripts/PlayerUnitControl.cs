using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerUnitControl : UnitBase
{
    Pathfinder pathFinder = new Pathfinder();
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
    }
}
