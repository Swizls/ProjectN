public class UIStateEnemyTurn : UIState
{
    public UIStateEnemyTurn(UIHandler handler) : base(handler)
    {
        _handler = handler;
    }

    public override void SetFlags()
    {
        _handler.TurnTitle.SetActive(true);

        _handler.UnitStats.SetActive(false);
        _handler.SelectPanel.SetActive(false);
        _handler.TurnButton.SetActive(false);
        _handler.Inventory.SetActive(false);
    }
}