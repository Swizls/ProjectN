using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShootAtTargetAction : IAction
{
    private ActionData data;

    private Unit _target;

    public ActionData Data => data;

    public ShootAtTargetAction(Unit target)
    {
        data = Resources.Load<ActionData>("ScriptableObjects/ActionData/ShootData");
        if (data == null)
            throw new System.Exception("Data for this action don't exsist");
        _target = target;
    }

    public bool TryExecute(Unit unit, ref int actionUnits)
    {
        if(ObstacleCheckForShot(unit.transform.position, _target.transform.position, unit.Tilemap) && unit.Actions.ActionUnits >= data.Cost)
        {
            _target.Health.ApplyDamage(unit.UnitDamage);
            actionUnits -= data.Cost;

            unit.Actions.Audio.clip = data.Clip;
            unit.Actions.Audio.Play();
            return true;
        }
        return false;
    }
    public bool ObstacleCheckForShot(Vector3 startPosFloat, Vector3 targetPosFloat, Tilemap tilemap)
    {
        List<Vector3Int> pointsList = GetShotTrajectory(startPosFloat, targetPosFloat, tilemap);

        foreach (Vector3Int point in pointsList)
        {
            RuleBaseTile tile = tilemap.GetTile<RuleBaseTile>(point);
            if (!tile.isPassable)
            {
                Debug.LogWarning("Obstacle check for a shot is: false! There is obstacle. Obstacle position: " + point);
                return false;
            }
        }
        return true;
    }
    private List<Vector3Int> GetShotTrajectory(Vector3 startPosFloat, Vector3 targetPosFloat, Tilemap tilemap)
    {
        List<Vector3Int> pointsList = new();

        Vector3Int startPosInt = tilemap.WorldToCell(startPosFloat);
        Vector3Int targetPosInt = tilemap.WorldToCell(targetPosFloat);

        Vector3 normalizedDireciton = (targetPosFloat - startPosFloat).normalized;
        Vector3Int roundedDirection = new((int)Mathf.Sign(normalizedDireciton.x), (int)Mathf.Sign(normalizedDireciton.y), 0);

        Vector3Int tileForCheck = startPosInt;

        while (tileForCheck.x != targetPosInt.x || tileForCheck.y != targetPosInt.y)
        {
            pointsList.Add(tileForCheck);
            if (tileForCheck.x != targetPosInt.x) tileForCheck.x += roundedDirection.x;
            if (tileForCheck.y != targetPosInt.y) tileForCheck.y += roundedDirection.y;
        }
        return pointsList;
    }

}
