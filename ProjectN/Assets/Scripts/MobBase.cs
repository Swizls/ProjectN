using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobBase : MonoBehaviour
{
    //��������������
    private int healthPoints = 100;
    [SerializeField]
    protected float speed = 5f;

    protected static bool isMoving = false;

    //����������
    protected static MapManager mapManager;
    protected static Tilemap tileMap;
    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        tileMap = FindObjectOfType<Tilemap>();
    }
    private void Update()
    {
        Death();
    }
    protected virtual IEnumerator UnitMovement(List<Vector3> waypoints)
    {
        //while (transform.position != waypoints[waypoints.Count - 1])
        //{
        //    for (int i = 0; i < waypoints.Count;)
        //    {
        //        if (transform.position != waypoints[i]) transform.position = Vector2.MoveTowards(transform.position, waypoints[i], speed);
        //        else i++;
        //        yield return new WaitForSeconds(1);
        //    }
        //}
        foreach (Vector3 waypoint in waypoints)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint, speed);
            yield return new WaitForSeconds(0.1f);
        }
        isMoving = false;
    }
    void Death()
    {
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
    protected List<Vector3> PathCalc(Vector3 targetPos)
    {
        List<Vector3> path = new List<Vector3>();
        Vector3Int targetCell = tileMap.WorldToCell(targetPos);
        Vector3Int currentWaypoint = tileMap.WorldToCell(transform.position);
        Vector3Int nextWaypointCheck;
        Vector3Int nextWaypoint = currentWaypoint;
        bool isTargetReachable = true;

        while (nextWaypoint != targetCell && isTargetReachable)
        {
            currentWaypoint = nextWaypoint;
            if (currentWaypoint.x != targetCell.x)
            {
                int direction = currentWaypoint.x > targetCell.x ? -1 : 1;
                nextWaypointCheck = new Vector3Int(nextWaypoint.x + direction, nextWaypoint.y, 0);
                if (!mapManager.isWalkable(tileMap.CellToWorld(nextWaypointCheck)))
                {
                    for (int i = 1; i < 20; i++)
                    {
                        nextWaypointCheck = new Vector3Int(nextWaypoint.x + direction, nextWaypoint.y + i, 0);
                        if (mapManager.isWalkable(tileMap.CellToWorld(nextWaypointCheck))) break;
                        nextWaypointCheck = new Vector3Int(nextWaypoint.x + direction, nextWaypoint.y - i, 0);
                        if (mapManager.isWalkable(tileMap.CellToWorld(nextWaypointCheck))) break;
                    }
                }
                if (mapManager.isWalkable(tileMap.CellToWorld(nextWaypointCheck))) nextWaypoint = new Vector3Int(nextWaypointCheck.x, nextWaypointCheck.y, 0);
                else isTargetReachable = false;
            }
            if (currentWaypoint.y != targetCell.y)
            {
                int direction = currentWaypoint.y > targetCell.y ? -1 : 1;
                nextWaypointCheck = new Vector3Int(nextWaypoint.x, nextWaypoint.y + direction, 0);
                if (!mapManager.isWalkable(tileMap.CellToWorld(nextWaypointCheck)))
                {
                    for (int i = 1; i < 20; i++)
                    {
                        nextWaypointCheck = new Vector3Int(nextWaypoint.x + i, nextWaypoint.y + direction, 0);
                        if (mapManager.isWalkable(tileMap.CellToWorld(nextWaypointCheck))) break;
                        nextWaypointCheck = new Vector3Int(nextWaypoint.x - i, nextWaypoint.y + direction, 0);
                        if (mapManager.isWalkable(tileMap.CellToWorld(nextWaypointCheck))) break;
                    }
                }
                if (mapManager.isWalkable(tileMap.CellToWorld(nextWaypointCheck))) nextWaypoint = new Vector3Int(nextWaypointCheck.x, nextWaypointCheck.y, 0);
                else isTargetReachable = false;
            }
            path.Add(tileMap.GetCellCenterWorld(nextWaypoint));
        }
        return path;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    foreach (Vector3 point in PathCalc(Camera.main.ScreenToWorldPoint(Input.mousePosition))) Gizmos.DrawSphere(point, 0.2f);
    //}
}
