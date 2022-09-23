using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobBase : MonoBehaviour
{
    //Характеристики
    private int healthPoints = 100;
    [SerializeField]
    private float speed = 5f;

    protected static bool isMoving = false;
    protected static Vector3Int unitPos;

    //Компоненты
    protected static MapManager mapManager;
    protected static Tilemap tileMap;
    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        tileMap = FindObjectOfType<Tilemap>();
    }
    private void Update()
    {
        unitPos = tileMap.WorldToCell(transform.position);
        Death();
    }
    protected virtual IEnumerator UnitMovement(List<PathNode> pathNodes)
    {
        foreach (PathNode pathNode in pathNodes)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(pathNode.x, pathNode.y), speed);
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
