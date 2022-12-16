using System.Collections;
using UnityEngine;


public class ReloadAction : IAction
{
    private ActionData _data;

    public ActionData Data => _data;

    public ReloadAction()
    {
        _data = Resources.Load<ActionData>("ScriptableObjects/ActionData/ReloadData");
        if (_data == null)
            throw new System.Exception("Data for shot action doesn't exsist");
    }

    public bool TryExecute(Unit unit, ref int actionUnits)
    {
        if (unit.Inventory.Weapon.TryReload() && actionUnits >= _data.Cost)
        {
            unit.Actions.Audio.clip = _data.Clip;
            unit.Actions.Audio.Play();

            actionUnits -= _data.Cost;
            return true;
        }

        return false;
    }
}
