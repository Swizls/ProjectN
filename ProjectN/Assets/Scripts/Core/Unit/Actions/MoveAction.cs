using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveAction : IAction
{
    private ActionData data;

    private Pathfinder _pathFinder = new Pathfinder();

    public ActionData Data => data;

    public MoveAction()
    {
        data = Resources.Load<ActionData>("ScriptableObjects/ActionData/MoveData");
        if(data == null)
            throw new System.Exception("Data for this action doesn't exsist");
    }
    public bool TryExecute(Unit unit, ref int currentActionUnits)
    {
        List<Vector3> path = GetPath(unit.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), unit.Tilemap);
        if (path != null && path.Count * data.Cost <= currentActionUnits)
        {
            currentActionUnits -= data.Cost * path.Count;
            unit.Movement.StartMove(path);
            return true;
        }
        return false;
    }
    private List<Vector3> GetPath(Vector3 position, Vector3 targetPos, Tilemap tilemap)
    {
        return _pathFinder.FindPath(position, targetPos, tilemap); ;
    }
}
