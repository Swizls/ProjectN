using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerUnitControl : MobBase
{
    GridLayout gridLayout;
    private static bool isMoving = false;
    public static bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }
    private void Start()
    {
        gridLayout = transform.parent.GetComponent<GridLayout>();
    }
    void Update()
    {
        UnitControl();
    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            StartCoroutine(UnitMovement(PathCalc(cellPosition)));
            Debug.Log(isMoving);
        }
    }


    Vector3Int cellPosition;
    protected void UnitControl()
    {
        if (Input.GetMouseButtonUp(0) && mapManager.isWalkable(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            isMoving = true;
            cellPosition = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
