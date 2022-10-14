using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitControl : MonoBehaviour
{
    private Pathfinder pathFinder = new Pathfinder();
    private PlayerUnit[] allPlayerUnits;
    private PlayerUnit selectedUnit;

    private void Start()
    {
        allPlayerUnits = FindObjectsOfType<PlayerUnit>();
        selectedUnit = allPlayerUnits[0];
    }

    private void Update()
    {
        UnitControl();
    }
    private void UnitControl()
    {
        if (Input.GetMouseButtonDown(1) && !selectedUnit.IsMoving)
        {
            selectedUnit.SetPath(GetPath());
        }
        if (Input.GetMouseButtonDown(0))
        {
            SelectUnit();
        }
    }
    private void SelectUnit()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(0, 0, -10),
                                            Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            selectedUnit = hit.collider.GetComponent<PlayerUnit>();
        }
    }
    private List<Vector3> GetPath()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<Vector3> path = pathFinder.FindPath(selectedUnit.transform.position, clickPos, selectedUnit.TileMap);

        return path;
    }
}