using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;
    public Action OpenInvetory;

    private bool _isInvetoryOpen = false;

    public bool IsInventoryOpen => _isInvetoryOpen;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        //Left mouse button
        if (Input.GetMouseButtonDown(0) && !PlayerUnitControl.Instance.CurrentSelectedUnit.Movement.IsMoving && !_isInvetoryOpen)
        {
            PlayerUnitControl.Instance.ExecuteOrder();
        }
        //Rigth mouse button
        if (Input.GetMouseButtonDown(1) && !PlayerUnitControl.Instance.CurrentSelectedUnit.Movement.IsMoving && !_isInvetoryOpen)
        {
            PlayerUnitControl.Instance.MoveOrder();
        }
        //Keyboard
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerUnitControl.Instance.ReloadOrder();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _isInvetoryOpen = !_isInvetoryOpen;
            OpenInvetory?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space) && EndTurnHandler.isPlayerTurn)
        {
            EndTurnHandler.EndTurn();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
