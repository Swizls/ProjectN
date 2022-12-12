public class UIStateGameUI : UIState 
{
    public UIStateGameUI(UIHandler handler) : base(handler)
    {
        _handler = handler;
    }

    public override void SetFlags()
    {
        _handler.UnitStats.SetActive(true);
        _handler.SelectPanel.SetActive(true);
        _handler.TurnButton.SetActive(true);

        _handler.Inventory.SetActive(false);
        _handler.TurnTitle.SetActive(false);
    }
}