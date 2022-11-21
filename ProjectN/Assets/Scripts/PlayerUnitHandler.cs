using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class PlayerUnitHandler : MonoBehaviour
{
    private const float PICKABLE_RANGE = 1.5f;

    private static List<Unit> allPlayerUnits;
    private static Unit _currentSelectedUnit;

    private static Tilemap _tileMap;
    private static LineRenderer _lineRenderer;
    private static GameObject _inventoryUI;
    private static Camera _mainCamera;

    private bool _isInvetoryOpen = false;

    public static List<Unit> AllPlayerUnits => allPlayerUnits;
    public static Unit CurrentSelectedUnit => _currentSelectedUnit;

    private void Awake()
    {
        _inventoryUI = Resources.FindObjectsOfTypeAll<InventoryUI>().First().gameObject;
        _tileMap = FindObjectOfType<Tilemap>();
        _lineRenderer = GetComponent<LineRenderer>();
        _mainCamera = Camera.main;

        allPlayerUnits = FindObjectsOfType<Unit>().Where(unit => unit.tag != "Enemy").ToList();
        _currentSelectedUnit = allPlayerUnits[0];
    }

    private void Update() => UnitControl();

    private void UnitControl()
    {
        //Left mouse button
        if (Input.GetMouseButtonDown(0) && !_currentSelectedUnit.Movement.IsMoving && !_isInvetoryOpen)
        { 
            RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition),
                                                 _mainCamera.ScreenToWorldPoint(Input.mousePosition));
            if(hit.collider != null)
            {
                if (IsUnit(hit))
                {
                    SelectUnit(hit);
                }
                else if(IsEnemey(hit))
                {
                    Unit enemy = GetTarget().GetComponent<Unit>();
                    _currentSelectedUnit.Actions.TryExecute(new ShootAtTargetAction(enemy));
                }
            }    
        }
        //Rigth mouse button
        if (Input.GetMouseButtonDown(1) && !_currentSelectedUnit.Movement.IsMoving && !_isInvetoryOpen)
        {
             _currentSelectedUnit.Actions.TryExecute(new MoveAction());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _isInvetoryOpen = !_isInvetoryOpen;
            _inventoryUI.SetActive(_isInvetoryOpen);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurnHandler.EndTurn();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private bool IsUnit(RaycastHit2D hit)
    {
        if (hit.collider.tag == "PlayerUnit") 
            return true;
        return false;
    }

    private bool IsEnemey(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Enemy")
            return true;
        return false;
    }
    public static void SelectUnit(Unit unit)
    {
        _currentSelectedUnit = unit;
        _currentSelectedUnit.unitValuesUpdated.Invoke();
    }
    private void SelectUnit(RaycastHit2D hit)
    {
        if (hit.collider.tag == "PlayerUnit")
            _currentSelectedUnit = hit.collider.GetComponent<Unit>();
        _currentSelectedUnit.unitValuesUpdated.Invoke();
    }
    private GameObject GetTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition),
                                             _mainCamera.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
            if (hit.collider.tag == "Enemy")
                return hit.collider.gameObject;
        return null;
    }
}