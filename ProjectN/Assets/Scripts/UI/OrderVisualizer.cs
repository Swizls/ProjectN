using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OrderVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Color _canMove;
    [SerializeField] private Color _cantMove;


    private Camera _mainCamera;
    private Pathfinder _pathfinder = new Pathfinder();
    private LineRenderer _lineRenderer;
    private Tilemap _tilemap;

    private List<GameObject> _enabledCells = new();
    private List<GameObject> _disabledCells = new();

    private void Start()
    {
        _mainCamera = Camera.main;
        _tilemap = FindObjectOfType<Tilemap>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (EndTurnHandler.isPlayerTurn && !PlayerUnitControl.Instance.CurrentUnit.Movement.IsMoving)
            VisualizePath();
        else
            HideCells();
    }

    private void VisualizePath()
    {
        HideCells();
        if (PlayerUnitControl.Instance.CurrentUnit != null)
        {
            Vector3[] path = _pathfinder.FindPath(PlayerUnitControl.Instance.CurrentUnit.transform.position,
                                                    _mainCamera.ScreenToWorldPoint(Input.mousePosition), _tilemap)?.ToArray();
            if (path != null)
            {
                if (path.Length > _disabledCells.Count)
                {
                    CreateNewCells(path);
                }
                ShowCells(path);
            }
        }
    }

    private void HideCells()
    {
        for (int i = 0; i < _enabledCells.Count; i++)
        {
            _enabledCells[i].SetActive(false);
            _disabledCells.Add(_enabledCells[i]);
            _enabledCells.Remove(_enabledCells[i]);
        }
    }

    private void ShowCells(Vector3[] path)
    {
        for (int i = _disabledCells.Count - 1; _enabledCells.Count < path.Length; i--)
        {
            _disabledCells[i].SetActive(true);
            _enabledCells.Add(_disabledCells[i]);
            _disabledCells.Remove(_disabledCells[i]);
        }
        for (int i = 0; i < path.Length; i++)
        {
            _enabledCells[i].transform.position = path[i];
            SetCellsColor();
        }
    }

    private void CreateNewCells(Vector3[] path)
    {
        for (int i = 0; i < path.Length - _disabledCells.Count; i++)
        {
            var cell = Instantiate(_prefab, transform);
            _enabledCells.Add(cell);
        }
    }
    private void SetCellsColor()
    {
        for(int i = 0; i < _enabledCells.Count; i++)
        {
            var spriteRenderer = _enabledCells[i].GetComponent<SpriteRenderer>();
            if (i * new MoveAction(_mainCamera.ScreenToWorldPoint(Input.mousePosition)).Data.Cost
                < PlayerUnitControl.Instance.CurrentUnit.Actions.ActionUnits)
            {
                spriteRenderer.color = _canMove;
            }
            else
            {
                spriteRenderer.color = _cantMove;
            }

        }
    }
}
