using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class PlayerUnitControl : UnitControl
{
    public static PlayerUnitControl Instance;

    private GameObject _inventoryUI;
    private Camera _mainCamera;

    private void Awake()
    {
        Instance = this;

        _inventoryUI = Resources.FindObjectsOfTypeAll<InventoryUI>().First().gameObject;
        _mainCamera = Camera.main;

        _allControlableUnits = FindObjectsOfType<Unit>().Where(unit => unit.CompareTag("PlayerUnit")).ToList();
        _currentUnit = _allControlableUnits[0];

        foreach (Unit unit in _allControlableUnits)
            unit.unitDied += OnUnitDeath;
    }

    public void ExecuteOrder()
    {
        RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition),
                                                 _mainCamera.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if (IsUnit(hit))
            {
                SelectUnit(hit);
            }
            else if (IsEnemey(hit))
            {
                Unit enemy = GetTarget().GetComponent<Unit>();
                _currentUnit.Actions.TryExecute(new ShootAction(enemy));
            }
        }
    }
    public void MoveOrder()
    {
        _currentUnit.Actions.TryExecute(new MoveAction(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }
    public void ReloadOrder()
    {
        _currentUnit.Actions.TryExecute(new ReloadAction());
    }

    private bool IsUnit(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("PlayerUnit")) 
            return true;
        return false;
    }

    private bool IsEnemey(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Enemy"))
            return true;
        return false;
    }

    private GameObject GetTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition),
                                             _mainCamera.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
            if (hit.collider.CompareTag("Enemy"))
                return hit.collider.gameObject;
        return null;
    }

    private void SelectUnit(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("PlayerUnit"))
            _currentUnit = hit.collider.GetComponent<Unit>();
        _currentUnit.unitValuesUpdated.Invoke();
    }

    public void SelectUnit(Unit unit)
    {
        _currentUnit = unit;
        _currentUnit.unitValuesUpdated.Invoke();
    }
}