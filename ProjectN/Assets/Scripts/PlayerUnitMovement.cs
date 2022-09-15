using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitMovement : MobBase
{
    void Update()
    {
        UnitMovement();
    }
    protected override void UnitMovement()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
            Vector3Int cellPosition = gridLayout.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            transform.position = gridLayout.CellToWorld(cellPosition);
            transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f);
        }
    }
}
