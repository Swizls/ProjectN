using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerUnitControl : MobBase
{
    void Update()
    {
        UnitControl();
    }
    protected void UnitControl()
    {
        if (Input.GetMouseButtonUp(0) && mapManager.isWalkable(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !isMoving)
        {
            isMoving = true;
            Vector3Int cellPosition = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            StartCoroutine(UnitMovement(PathCalc(cellPosition)));
        }
    }
}
