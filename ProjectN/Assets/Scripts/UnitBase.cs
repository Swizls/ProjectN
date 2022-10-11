using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitBase : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    protected static bool isMoving = false;
    protected static Vector3Int unitPos;

    protected static Tilemap tileMap;
    [SerializeField]
    protected static ITilemap iTilemap;

    protected static List<Vector3> pathList;
    private void Start()
    {
        tileMap = FindObjectOfType<Tilemap>();
    }
    private void Update()
    {
        unitPos = tileMap.WorldToCell(transform.position);
        if (isMoving)
        {
            UnitMovement();
        }
    }
    int currentPathIndex = 0;
    protected void UnitMovement()
    {
        if (pathList != null && Vector3.Distance(transform.position, pathList[pathList.Count - 1]) > 0.01f)
        {
            if (isMoving)
            {
                if (Vector3.Distance(transform.position, pathList[currentPathIndex]) > 0.05f)
                {
                    Vector3 moveDir = (pathList[currentPathIndex] - transform.position).normalized;
                    transform.position = transform.position + moveDir * speed * Time.deltaTime;
                }
                else
                {
                    currentPathIndex++;
                }
                if(currentPathIndex >= pathList.Count)
                {
                    currentPathIndex = 0;
                    isMoving = false;
                }
            }
        }
        else
        {
            currentPathIndex = 0;
            isMoving = false;
        }
    }
}
