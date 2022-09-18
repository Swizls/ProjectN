using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobBase : MonoBehaviour
{
    private int healthPoints = 100;
    [SerializeField]
    protected float speed = 5f;
    private Vector3Int mobPos;
    protected static MapManager mapManager;
    protected static Tilemap tileMap;
    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log(mapManager);
        tileMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
    }
    private void Update()
    {
        Death();
    }
    protected virtual void UnitMovement()
    {
    }
    void Death()
    {
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
    List<Vector3> path = new List<Vector3>();
    protected void pathCalc(Vector3Int targetCell)
    {
        Vector3 targetPos = tileMap.GetCellCenterWorld(targetCell);
        mobPos = tileMap.WorldToCell(transform.position);


        Vector3Int waypointProjection = mobPos;
        while (waypointProjection != targetCell)
        {
            if (mobPos.x > targetCell.x)
            {
                mobPos = new Vector3Int(mobPos.x - 1, mobPos.y, 0);
                Vector3 cellPosInWorld = tileMap.GetCellCenterWorld(mobPos);
                path.Add(cellPosInWorld);
                waypointProjection = mobPos;
            }
            else if (mobPos.x < targetCell.x)
            {
                mobPos = new Vector3Int(mobPos.x + 1, mobPos.y, 0);
                Vector3 cellPosInWorld = tileMap.GetCellCenterWorld(mobPos);
                path.Add(cellPosInWorld);
                waypointProjection = mobPos;
            }
            if (mobPos.y > targetCell.y)
            {
                mobPos = new Vector3Int(mobPos.x, mobPos.y - 1, 0);
                Vector3 cellPosInWorld = tileMap.GetCellCenterWorld(mobPos);
                path.Add(cellPosInWorld);
                waypointProjection = mobPos;
            }
            else if (mobPos.y < targetCell.y)
            {
                mobPos = new Vector3Int(mobPos.x, mobPos.y + 1, 0);
                Vector3 cellPosInWorld = tileMap.GetCellCenterWorld(mobPos);
                path.Add(cellPosInWorld);
                waypointProjection = mobPos;
            }
            else break;
        }
        foreach (Vector3 waypoint in path)
        {
            if (transform.position != path[path.Count - 1]) transform.position = Vector2.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
            else
            {
                PlayerUnitControl.IsMoving = false;
                path.Clear();
            }
        }
        //for(; transform.position != path[path.Count - 1];)
        //{
        //    for(int i = 0; i < path.Count;)
        //    {
        //        if (transform.position != path[i]) transform.position = Vector2.MoveTowards(transform.position, path[i], speed * Time.deltaTime);
        //        else i++;
        //    }
        //}
        //PlayerUnitControl.IsMoving = false;
        //path.Clear();
    }
}
