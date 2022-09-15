using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitMovement : MobBase
{
    Vector3 targetPos = new Vector3();
    bool isMoving = false;
    void Update()
    {
        UnitMovement();
    }
    protected override void UnitMovement()
    {
        GridLayout gridLayout;
        Vector3Int cellPosition;
        if (Input.GetMouseButtonUp(0))
        {
            gridLayout = transform.parent.GetComponentInParent<GridLayout>();
            cellPosition = gridLayout.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            targetPos = gridLayout.CellToWorld(cellPosition);
            targetPos = new Vector2(targetPos.x + 0.5f, targetPos.y + 0.5f);
            isMoving = true;
        }
        if (isMoving && transform.position != targetPos) transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        //else isMoving = false;
    }
}
