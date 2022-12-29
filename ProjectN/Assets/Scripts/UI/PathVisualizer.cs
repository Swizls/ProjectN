using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathVisualizer : MonoBehaviour
{
    private Camera _mainCamera;
    private Pathfinder _pathfinder = new Pathfinder();
    private LineRenderer _lineRenderer;
    private Tilemap _tilemap;

    private void Start()
    {
        _mainCamera = Camera.main;
        _tilemap = FindObjectOfType<Tilemap>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (EndTurnHandler.isPlayerTurn)
        {
            VisualizePath();
        }
        else
        {
            _lineRenderer.positionCount = 0;
        }
    }

    private void VisualizePath()
    {
        if (PlayerUnitControl.Instance.CurrentUnit != null)
        {
            Vector3[] path = _pathfinder.FindPath(PlayerUnitControl.Instance.CurrentUnit.transform.position,
                                                    _mainCamera.ScreenToWorldPoint(Input.mousePosition), _tilemap)?.ToArray();
            if (path != null)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    path[i] = new Vector3(path[i].x, path[i].y, -1);
                }

                _lineRenderer.positionCount = path.Length;
                _lineRenderer.SetPositions(path);

                if (PlayerUnitControl.Instance.CurrentUnit.Actions.ActionUnits >= path.Length * new MoveAction(Input.mousePosition).Data.Cost)
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
    }
}
