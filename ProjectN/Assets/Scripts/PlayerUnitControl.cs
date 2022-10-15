using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerUnitControl : MonoBehaviour
{
    private Pathfinder pathFinder = new Pathfinder();
    private IEnumerable<UnitBase> allPlayerUnits;
    private UnitBase selectedUnit;

    private void Start()
    {
        allPlayerUnits = FindObjectsOfType<UnitBase>().ToList().Where(unit => unit.tag != "Enemy");
        selectedUnit = allPlayerUnits.First();
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
            if (!IsEnemeyInCell())
            {
                SelectUnit();
            }
            else
            {
                selectedUnit.ShootAtTarget(GetTarget());
            }
        }
    }
    private void SelectUnit()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(0, 0, -10),
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if(hit.collider.tag == "PlayerUnit")
            { 
                selectedUnit = hit.collider.GetComponent<UnitBase>();
            }
        }
    }
    private List<Vector3> GetPath()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<Vector3> path = pathFinder.FindPath(selectedUnit.transform.position, clickPos, selectedUnit.TileMap);

        return path;
    }
    private UnitBase GetTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(0, 0, -10),
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if(hit.collider.tag == "Enemy")
            { 
                return hit.collider.GetComponent<UnitBase>();
            }
        }
        return null;
    }
    private bool IsEnemeyInCell()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(0, 0, -10),
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy")
            {
                return true;
            }
        }
        return false;
    }
}