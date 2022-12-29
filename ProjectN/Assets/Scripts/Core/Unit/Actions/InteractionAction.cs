using System;
using UnityEngine;
public class InteractionAction : IAction
{
    private ActionData _data;

    private Unit _target;

    public ActionData Data => _data;

    public InteractionAction(Unit target)
    {
        _data = Resources.Load<ActionData>("ScriptableObjects/ActionData/" + nameof(InteractionAction));
        if (_data == null)
            throw new System.Exception("Data for shot action doesn't exsist");

        _target = target;
    }

    public bool TryExecute(Unit unit, ref int actionUnits)
    {
        actionUnits = _data.Cost;
        return true;
    }
}
