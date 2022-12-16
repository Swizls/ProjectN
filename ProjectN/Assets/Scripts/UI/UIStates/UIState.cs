public abstract class UIState
{
    protected UIHandler _handler;

    public UIState(UIHandler handler)
    {
        _handler = handler;
    }

    public abstract void SetFlags();
}