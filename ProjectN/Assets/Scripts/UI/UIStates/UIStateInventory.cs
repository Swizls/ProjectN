using System.Collections;
using UnityEngine;

public class UIStateInventory : UIState
{
    public UIStateInventory(UIHandler handler) : base(handler)
    {
        _handler = handler;
    }

    public override void SetFlags()
    {
        _handler.Inventory.SetActive(true);

        _handler.UnitStats.SetActive(false);
        _handler.SelectPanel.SetActive(false);
        _handler.TurnTitle.SetActive(false);
        _handler.TurnButton.SetActive(false);
    }
}