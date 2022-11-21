using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public ActionData Data { get; }
    public bool TryExecute(Unit unit, ref int actionUnits);
}
