using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitBase : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int healthPoints;
    [SerializeField] private int unitDamage;

    private readonly int startActionPoints = 20;
    private int currentActionPoints;

    private bool isMoving = false;

    private Tilemap tileMap;
    private List<Vector3> pathList;
    private SpriteRenderer sprite;

    public bool IsMoving => isMoving;
    public Tilemap TileMap => tileMap;
    public int UnitDamage => unitDamage;
    public int ActionPoints => currentActionPoints;

    private void Start()
    {
        currentActionPoints = startActionPoints;
        tileMap = FindObjectOfType<Tilemap>();
        sprite = GetComponentInChildren<SpriteRenderer>();
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
        const float MIN_DISTANCE = 0.05f;
        if (pathList != null && pathList.Count != 0)
        {
            if (isMoving && Vector3.Distance(transform.position, pathList[pathList.Count - 1]) > MIN_DISTANCE)
            {
                if (Vector3.Distance(transform.position, pathList[currentPathIndex]) > MIN_DISTANCE)
                {
                    Vector3 moveDirerction = (pathList[currentPathIndex] - transform.position).normalized;
                    transform.position = transform.position + moveDirerction * speed * Time.deltaTime;
                    if(Mathf.Round(moveDirerction.x) != 1)
                        sprite.flipX = true ;
                    else
                        sprite.flipX = false;
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
    private void OnTurnEnd()
    {
        currentActionPoints = startActionPoints;
    }

    public void ShootAtTarget(GameObject target)
    {
        target.GetComponent<UnitBase>().TakeDamage(unitDamage);
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