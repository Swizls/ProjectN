using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerUnitControl : MonoBehaviour
{
    private Pathfinder pathFinder = new Pathfinder();

    private List<UnitBase> allPlayerUnits;
    private UnitBase selectedUnit;

    public List<UnitBase> AllPlayerUnits => allPlayerUnits;

    private void Awake()
    {
        allPlayerUnits = FindObjectsOfType<UnitBase>().Where(unit => unit.tag != "Enemy").ToList();
        selectedUnit = allPlayerUnits[0];
    }

    private void Update()
    {
        UnitControl();
    }

    private void UnitControl()
    {
        //Rigth mouse button
        if (Input.GetMouseButtonDown(1) && !selectedUnit.IsMoving)
        {
            List<Vector3> path = GetPath();
            selectedUnit.SetPath(path);
        }
        //Left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsEnemeyInCell())
            {
                SelectUnit();
            }
            else
            {
                GameObject enemy = GetTarget();
                selectedUnit.ShootAtTarget(enemy);
            }
        }
    }
    private void SelectUnit()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if(hit.collider.tag == "PlayerUnit")
            { 
                selectedUnit = hit.collider.GetComponent<UnitBase>();
            }
        }
    }
    public void SelectUnit(UnitBase unit)
    {
        selectedUnit = unit;
    }
    private List<Vector3> GetPath()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<Vector3> path = pathFinder.FindPath(selectedUnit.transform.position, clickPos, selectedUnit.TileMap);

        return path;
    }
    private GameObject GetTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if(hit.collider.tag == "Enemy")
            { 
                return hit.collider.gameObject;
            }
        }
        return null;
    }
    private bool IsEnemeyInCell()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
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