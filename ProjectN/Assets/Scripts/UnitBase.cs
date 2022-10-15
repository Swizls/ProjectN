using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitBase : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int healthPoints;
    [SerializeField] private int unitDamage;

    private bool isMoving = false;
    private Tilemap tileMap;
    private List<Vector3> pathList;

    public bool IsMoving
    {
        get { return isMoving; }
    }
    public Tilemap TileMap
    {
        get { return tileMap; }
    }
    public int UnitDamage
    {
        get { return unitDamage; }
    }

    private void Start()
    {
        tileMap = FindObjectOfType<Tilemap>();
    }
    private void Update()
    {
        if (isMoving)
        {
            UnitMovement();
        }
    }
    int currentPathIndex = 0;
    private void UnitMovement()
    {
        if (pathList != null && pathList.Count != 0)
        {
            if (isMoving && Vector3.Distance(transform.position, pathList[pathList.Count - 1]) > 0.01f)
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
                    StopMoving();
                }
            }
            else
            {
                StopMoving();
            }
        }
        else
        {
            StopMoving();
        }
    }
    private void StopMoving()
    {
        currentPathIndex = 0;
        isMoving = false;
    }
    public void SetPath(List<Vector3> pathList)
    {
        this.pathList = pathList;
        isMoving = true;
    }
    private void Death()
    {
        Destroy(gameObject);
    }
    public void ShootAtTarget(UnitBase target)
    {
        target.TakeDamage(unitDamage);
    }
    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        Debug.Log('"'+ tag + '"' + " health points: " + healthPoints);
        if(healthPoints <= 0)
        {
            Death();
        }
    }
}
