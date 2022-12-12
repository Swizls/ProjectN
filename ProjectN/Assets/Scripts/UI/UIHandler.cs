using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    private UIState _currentState;

    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _unitStats;
    [SerializeField] private GameObject _selectPanel;
    [SerializeField] private GameObject _turnTitle;
    [SerializeField] private GameObject _turnButton;

    private List<UIState> _states;

    public GameObject Inventory => _inventory;
    public GameObject UnitStats => _unitStats;
    public GameObject SelectPanel => _selectPanel;
    public GameObject TurnTitle => _turnTitle;
    public GameObject TurnButton => _turnButton;

    private void Start()
    {
        _states = new List<UIState>() { new UIStateInventory(this), 
                                        new UIStateGameUI(this),
                                        new UIStateEnemyTurn(this) };

        _currentState = new UIStateGameUI(this);
        _currentState.SetFlags();

        PlayerUnitControl.Instance.OpenInvetory += OnOpenInvetory;
    }

    private void OnEnable()
    {
        EndTurnHandler.enemyTurn += OnTurnEnd;
        EndTurnHandler.playerTurn += OnTurnEnd;

        if(PlayerUnitControl.Instance != null)
            PlayerUnitControl.Instance.OpenInvetory += OnOpenInvetory;
    }

    private void OnDisable()
    {
        EndTurnHandler.enemyTurn -= OnTurnEnd;
        EndTurnHandler.playerTurn -= OnTurnEnd;

        PlayerUnitControl.Instance.OpenInvetory -= OnOpenInvetory;
    }

    private void OnTurnEnd()
    {
        if(EndTurnHandler.isPlayerTurn)
            SwitchState<UIStateGameUI>();
        else
            SwitchState<UIStateEnemyTurn>();
    }

    private void OnOpenInvetory()
    {
        if(PlayerUnitControl.Instance.IsInventoryOpen)
            SwitchState<UIStateInventory>();
        else
            SwitchState<UIStateGameUI>();
    }

    private void SwitchState<T>() where T : UIState
    {
        _currentState = _states.FirstOrDefault(s => s is T);
        _currentState.SetFlags();
    }
}
