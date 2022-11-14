using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class PlayerUnitHandler : MonoBehaviour
{
    private const float PICKABLE_RANGE = 1.5f;

    private Pathfinder _pathFinder = new Pathfinder();

    private static List<UnitBehaviour> allPlayerUnits;
    private static UnitBehaviour _currentSelectedUnit;

    private static Tilemap _tileMap;
    private static LineRenderer _lineRenderer;
    private static GameObject _inventoryUI;
    private bool _isInvetoryOpen = false;

    public static List<UnitBehaviour> AllPlayerUnits => allPlayerUnits;
    public static UnitBehaviour CurrentSelectedUnit => _currentSelectedUnit;

    private void Awake()
    {
        _inventoryUI = Resources.FindObjectsOfTypeAll<InventoryUI>().First().gameObject;
        _tileMap = FindObjectOfType<Tilemap>();
        _lineRenderer = GetComponent<LineRenderer>();

        allPlayerUnits = FindObjectsOfType<UnitBehaviour>().Where(unit => unit.tag != "Enemy").ToList();
        _currentSelectedUnit = allPlayerUnits[0];
    }

    private void Update()
    {
        if (!_isInvetoryOpen && _currentSelectedUnit != null)
        {
            ShowPath();
        }
        UnitControl();
    }

    private void ShowPath()
    {
        Vector3[] path = GetPath()?.ToArray();
        if(path != null)
        {
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = new Vector3(path[i].x, path[i].y, -1);
            }

            _lineRenderer.positionCount = path.Length;
            _lineRenderer.SetPositions(path);

            if (_currentSelectedUnit.ActionUnits >= path.Length * UnitBehaviour.MOVE_COST)
            {
                _lineRenderer.startColor = Color.green;
                _lineRenderer.endColor = Color.green;
            }
            else
            {
                _lineRenderer.startColor = Color.red;
                _lineRenderer.endColor = Color.red;
            }
        }
        else
        {
            _lineRenderer.positionCount = 0;
        }
    }
    private void UnitControl()
    {
        //Left mouse button
        if (Input.GetMouseButtonDown(0) && !_currentSelectedUnit.IsMoving && !_isInvetoryOpen)
        { 
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                                 Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(hit.collider != null)
            {
                if (IsUnit(hit))
                {
                    SelectUnit(hit);
                }
                else if (IsEnemey(hit) && _currentSelectedUnit.ActionUnits >= UnitBehaviour.SHOT_COST)
                {
                    GameObject enemy = GetTarget();
                    if(_currentSelectedUnit.ObstacleCheckForShot(_currentSelectedUnit.transform.position, enemy.transform.position))
                    {
                        _currentSelectedUnit.ShootAtTarget(enemy);
                    }
                }
            }    
        }
        //Rigth mouse button
        if (Input.GetMouseButtonDown(1) && !_currentSelectedUnit.IsMoving && !_isInvetoryOpen)
        {   
            List<Vector3> path = GetPath();
            if(path != null)
            { 
                if(_currentSelectedUnit.ActionUnits >= GetPath().Count * UnitBehaviour.MOVE_COST)
                    _currentSelectedUnit.StartMove(path);
            }
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
    public static void SelectUnit(UnitBehaviour unit)
    {
        _currentSelectedUnit = unit;
        _currentSelectedUnit.unitValuesUpdated.Invoke();
    }
    private void SelectUnit(RaycastHit2D hit)
    {
        if (hit.collider.tag == "PlayerUnit")
            _currentSelectedUnit = hit.collider.GetComponent<UnitBehaviour>();
        _currentSelectedUnit.unitValuesUpdated.Invoke();
    }
    private List<Vector3> GetPath()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        List<Vector3> path = _pathFinder.FindPath(_currentSelectedUnit.transform.position, clickPos, _tileMap);

        return path;
    }
    private GameObject GetTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy")
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
}