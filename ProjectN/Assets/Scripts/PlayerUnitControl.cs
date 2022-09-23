using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerUnitControl : MobBase
{
    Pathfinder pathFinder = new Pathfinder();
    void Update()
    {
        UnitControl();
    }
    protected void UnitControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMoving = true;
            Vector3Int cellPosition = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            List<Vector3> path = pathFinder.FindPath(cellPosition, unitPos, tileMap);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                    Debug.DrawLine(path[i], path[i + 1], Color.red, 2f);
            }
        }
    }
}
