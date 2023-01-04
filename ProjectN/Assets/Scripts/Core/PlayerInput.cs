using System;
using System.Collections.Generic;
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
        if (Input.GetMouseButtonDown(0) && !PlayerUnitControl.Instance.CurrentUnit.Movement.IsMoving && !_isInvetoryOpen)
        {
            PlayerUnitControl.Instance.ExecuteOrder();
        }
        //Rigth mouse button
        if (Input.GetMouseButtonDown(1) && !PlayerUnitControl.Instance.CurrentUnit.Movement.IsMoving && !_isInvetoryOpen)
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
            OpenInventory();
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerUnitControl.Instance.HealOrder();
        }
    }

    public void OpenInventory()
    {
        PlayerUnitControl.Instance.CurrentUnit.Inventory.GetItemsOnGround();
        _isInvetoryOpen = !_isInvetoryOpen;
        OpenInvetory?.Invoke();
    }
}
