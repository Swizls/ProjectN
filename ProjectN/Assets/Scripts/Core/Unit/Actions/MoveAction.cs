using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveAction : IAction
{
    private ActionData data;

    private readonly Vector3 _targetPosition;

    private readonly Pathfinder _pathFinder = new Pathfinder();

    public ActionData Data => data;

    public MoveAction(Vector3 targetPosition)
    {
        data = Resources.Load<ActionData>("ScriptableObjects/ActionData/MoveData");
        if(data == null)
            throw new System.Exception("Data for move action doesn't exsist");

        _targetPosition = targetPosition;
    }
    public bool TryExecute(Unit unit, ref int currentActionUnits)
    {
        int maxPathLength = currentActionUnits / data.Cost;

        List<Vector3> path = GetPath(unit.transform.position, _targetPosition, unit.Tilemap);
        if (path != null)
        {
            if(path.Count > maxPathLength)
            {
                for(int i = path.Count - 1; i >= maxPathLength; i--)
                {
                    path.Remove(path[i]);
                }
            }
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
