using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobBase : MonoBehaviour
{
    //��������������
    private int healthPoints = 100;
    [SerializeField]
    private float speed = 5f;

    protected static bool isMoving = false;
    protected static Vector3Int unitPos;

    //����������
    protected static Tilemap tileMap;
    [SerializeField]
    protected static ITilemap iTilemap;
    private void Start()
    {
        tileMap = FindObjectOfType<Tilemap>();
    }
    private void Update()
    {
        unitPos = tileMap.WorldToCell(transform.position);
        Death();
    }
    protected virtual IEnumerator UnitMovement(List<Vector3> path)
    {
        foreach (Vector3 waypoint in path)
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
}
