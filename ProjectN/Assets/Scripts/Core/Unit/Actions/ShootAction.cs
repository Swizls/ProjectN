using System.Collections.Generic;
using UnityEngine;
using ShotUtilites;

public class ShootAction : IAction
{
    private ActionData _data;

    private Unit _target;

    public ActionData Data => _data;

    public ShootAction(Unit target)
    {
        _data = Resources.Load<ActionData>("ScriptableObjects/ActionData/" + nameof(ShootAction));
        if (_data == null)
            throw new System.Exception("Data for shot action doesn't exsist");

        _target = target;
    }

    public bool TryExecute(Unit unit, ref int actionUnits)
    {
        if(ShotUtilities.ObstacleCheckForShot(unit.transform.position, _target.transform.position, unit.Tilemap) && unit.Actions.ActionUnits >= _data.Cost)
        {
            List<Vector3Int> shotTrajectory = ShotUtilities.GetShotTrajectory(unit.transform.position, _target.transform.position, unit.Tilemap);

            if (unit.Inventory.Weapon.TryShoot(_target, shotTrajectory.Count, ShotUtilities.GetCoversOnTrajectory(shotTrajectory)))
            {
                actionUnits -= _data.Cost;

                if(unit.transform.position.x <= _target.transform.position.x)
                {
                    unit.Movement.Sprite.flipX = false;
                }
                else
                {
                    unit.Movement.Sprite.flipX = true;
                }

                unit.Actions.Audio.clip = _data.Clip;
                unit.Actions.Audio.Play();
                return true;
            }
        }
        return false;
    }
}