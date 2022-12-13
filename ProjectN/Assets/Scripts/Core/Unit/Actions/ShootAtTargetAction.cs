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
            throw new System.Exception("Data for shot action doesn't exsist");

        _target = target;
    }

    public bool TryExecute(Unit unit, ref int actionUnits)
    {
        if(ObstacleCheckForShot(unit.transform.position, _target.transform.position, unit.Tilemap) && unit.Actions.ActionUnits >= data.Cost)
        {
            if(unit.Inventory.Weapon.TryShoot(_target))
            {
                actionUnits -= data.Cost;

                if(unit.transform.position.x <= _target.transform.position.x)
                {
                    unit.Movement.Sprite.flipX = false;
                }
                else
                {
                    unit.Movement.Sprite.flipX = true;
                }

                unit.Actions.Audio.clip = data.Clip;
                unit.Actions.Audio.Play();
                return true;
            }
        }
        return false;
    }

    public bool ObstacleCheckForShot(Vector3 startPosFloat, Vector3 targetPosFloat, Tilemap tilemap)
    {
        foreach (Vector3Int point in GetShotTrajectory(startPosFloat, targetPosFloat, tilemap))
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

    private List<Vector3Int> GetShotTrajectory(Vector3 startPos, Vector3 targetPos, Tilemap tilemap)
    {
        List<Vector3Int> pointsList = new();
        Vector3Int targetPosInt = tilemap.WorldToCell(targetPos);
        Vector3Int currentPointInt = tilemap.WorldToCell(startPos);

        pointsList.Add(currentPointInt);

        Vector3 direction = (targetPos - startPos).normalized;
        Vector3 nextpoint = currentPointInt;

        while(currentPointInt != targetPosInt)
        {
            if(currentPointInt.x != targetPosInt.x)
                nextpoint.x += direction.x;
            if(currentPointInt.y != targetPosInt.y)
                nextpoint.y += direction.y;

            currentPointInt = tilemap.WorldToCell(nextpoint);
            pointsList.Add(currentPointInt);
        }
        return pointsList;
    }

}
