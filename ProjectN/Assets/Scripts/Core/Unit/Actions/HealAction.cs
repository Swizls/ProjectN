using UnityEngine;

public class HealAction : IAction
{
    private ActionData _data;
    private const string path = "ScriptableObjects/ActionData/";

    public ActionData Data => _data;

    public HealAction()
    {
        _data = Resources.Load<ActionData>(path + nameof(HealAction));
        if (_data == null)
            throw new System.Exception($"Data for {nameof(HealAction)} doesn't exsist");
    }

    public bool TryExecute(Unit unit, ref int actionUnits)
    {
        if (unit.Inventory.IsHealItemInInventory(out MedicineInfo healItem))
        {
            unit.Health.Heal(healItem.HealPoints);
            unit.Inventory.ItemRemove(healItem);

            unit.Actions.Audio.clip = _data.Clip;
            unit.Actions.Audio.Play();

            actionUnits -= _data.Cost;
            return true;
        }

        return false;
    }
}