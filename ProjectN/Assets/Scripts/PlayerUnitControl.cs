using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerUnitControl : MobBase
{
    MapManager mapManager;
    GridLayout gridLayout;
    bool isMoving = false;
    void Update()
    {
        gridLayout = transform.parent.GetComponent<GridLayout>();
        mapManager = FindObjectOfType<MapManager>();
        UnitMovement();
    }

    Vector3 targetPos = new Vector3();
    protected override void UnitMovement()
    {
        Tilemap tileMap;
        Vector3Int cellPosition;
        if (Input.GetMouseButtonUp(0) && mapManager.isWalkable(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            isMoving = true;
            tileMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
            cellPosition = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            targetPos = tileMap.GetCellCenterWorld(cellPosition);
        }
        if (isMoving && transform.position != targetPos) transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
