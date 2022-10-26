using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitBase : MonoBehaviour
{
    public const int MOVE_COST = 2;
    public const int SHOT_COST = 5;
    public const int INTERACTION_COST = 3;

    [SerializeField] private float speed;

    [SerializeField] private int unitHealth;
    [SerializeField] private int unitDamage;

    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip pickupClip;

    private readonly int startActionPoints = 20;
    private int currentActionUnits;
    private int currentPathIndex = 0;

    private bool isAudioPlayed = false;
    private bool isMoving = false;

    private List<Vector3> pathList;

    private Tilemap tileMap;
    private SpriteRenderer sprite;
    private AudioSource audioComponent;
    private UnitInventory inventory;

    public Action unitValuesUpdated;

    public bool IsMoving => isMoving;
    public Tilemap TileMap => tileMap;
    public int UnitDamage => unitDamage;
    public int UnitHealth => unitHealth;
    public int ActionUnits => currentActionUnits;
    public UnitInventory Inventory => inventory;


    private void Start()
    {
        currentActionUnits = startActionPoints;

        audioComponent = GetComponent<AudioSource>();
        tileMap = FindObjectOfType<Tilemap>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        inventory = GetComponent<UnitInventory>();

        unitValuesUpdated?.Invoke();
    }

    private void OnEnable()
    {
        EndTurnHandler.turnEnd += OnTurnEnd;
    }

    private void OnDisable()
    {
        EndTurnHandler.turnEnd -= OnTurnEnd;
    }

    private void Update()
    {
        if (isMoving)
        {
            UnitMovement();
        }
    }

    private void UnitMovement()
    {
        const float MIN_DISTANCE = 0.05f;
        if (!isAudioPlayed)
        {
            audioComponent.clip = walkClip;
            audioComponent.Play();
            isAudioPlayed = true;
        }
        if (pathList != null &&pathList.Count != 0 && isMoving && currentActionUnits > 0)
        {
            if (Vector3.Distance(transform.position, pathList[pathList.Count - 1]) > MIN_DISTANCE)
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
                    currentActionUnits -= MOVE_COST;
                    unitValuesUpdated.Invoke();
                    currentPathIndex++;
                }

                if(currentPathIndex >= pathList.Count)
                {
                    StopMoving();
                }
            }
            else
            {
                currentActionUnits -= MOVE_COST;
                unitValuesUpdated.Invoke();
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
        audioComponent.Stop();
        isAudioPlayed = false;
    }
    public void StartMove(List<Vector3> pathList)
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
        currentActionUnits = startActionPoints;
        unitValuesUpdated?.Invoke();
    }

    public void PickupItem(GameObject item)
    {
        inventory.AddItem(item.GetComponent<Item>().Info);
        Destroy(item);
        currentActionUnits -= INTERACTION_COST;

        audioComponent.clip = pickupClip;
        audioComponent.Play();

        unitValuesUpdated?.Invoke();
    }

    public void ShootAtTarget(GameObject target)
    {
        target.GetComponent<UnitBase>().TakeDamage(unitDamage);
        currentActionUnits -= SHOT_COST;

        unitValuesUpdated?.Invoke();

        audioComponent.Stop();
        audioComponent.clip = shotClip;
        audioComponent.Play();
    }

    public void TakeDamage(int damage)
    {
        unitHealth -= damage;
        unitValuesUpdated?.Invoke();

        if(unitHealth <= 0)
        {
            Death();
        }
    }
}