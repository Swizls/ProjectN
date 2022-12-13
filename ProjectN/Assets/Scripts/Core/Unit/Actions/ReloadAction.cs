using System.Collections;
using UnityEngine;


public class ReloadAction : IAction
{
    private ActionData _data;

    public ActionData Data => _data;

    public bool TryExecute(Unit unit, ref int actionUnits)
    {
        unit.Inventory.Weapon.Reload();
        return true;
    }
}
