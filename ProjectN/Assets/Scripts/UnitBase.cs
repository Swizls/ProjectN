using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitBase : MonoBehaviour
{
    public const int MOVE_COST = 2;
    public const int SHOT_COST = 5;
    public const int INTERACTION_COST = 3;

    [SerializeField] private float _speed;

    [SerializeField] private int _unitHealth;
    [SerializeField] private int _unitDamage;

    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _shotClip;
    [SerializeField] private AudioClip _pickupClip;

    private readonly int _startActionPoints = 20;
    private int _currentActionUnits;
    private int _currentPathIndex = 0;

    private bool _isAudioPlayed = false;
    private bool _isMoving = false;

    private List<Vector3> _pathList;

    private Tilemap _tileMap;
    private SpriteRenderer _sprite;
    private AudioSource _audioComponent;
    private UnitInventory _inventory;

    public Action unitValuesUpdated;

    public bool IsMoving => _isMoving;
    public Tilemap TileMap => _tileMap;
    public int UnitDamage => _unitDamage;
    public int UnitHealth => _unitHealth;
    public int ActionUnits => _currentActionUnits;
    public UnitInventory Inventory => _inventory;


    private void Start()
    {
        _currentActionUnits = _startActionPoints;

        _audioComponent = GetComponent<AudioSource>();
        _tileMap = FindObjectOfType<Tilemap>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _inventory = GetComponent<UnitInventory>();

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
        if (_isMoving)
        {
            UnitMovement();
        }
    }

    private void UnitMovement()
    {
        const float MIN_DISTANCE = 0.07f;
        if (!_isAudioPlayed)
        {
            _audioComponent.clip = _walkClip;
            _audioComponent.Play();
            _isAudioPlayed = true;
        }
        if (_pathList != null &&_pathList.Count != 0 && _isMoving && _currentActionUnits > 0)
        {
            if (Vector3.Distance(transform.position, _pathList[_pathList.Count - 1]) > MIN_DISTANCE)
            {
                if (Vector3.Distance(transform.position, _pathList[_currentPathIndex]) > MIN_DISTANCE)
                {
                    Vector3 moveDirerction = (_pathList[_currentPathIndex] - transform.position).normalized;
                    transform.position = transform.position + moveDirerction * _speed * Time.deltaTime;
                    if(Mathf.Round(moveDirerction.x) != 1)
                        _sprite.flipX = true ;
                    else
                        _sprite.flipX = false;
                }
                else
                {
                    _currentActionUnits -= MOVE_COST;
                    unitValuesUpdated.Invoke();
                    _currentPathIndex++;
                }

                if(_currentPathIndex >= _pathList.Count)
                {
                    StopMoving();
                }
            }
            else
            {
                _currentActionUnits -= MOVE_COST;
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
        _currentPathIndex = 0;
        _isMoving = false;
        _audioComponent.Stop();
        _isAudioPlayed = false;
    }
    public void StartMove(List<Vector3> pathList)
    {
        this._pathList = pathList;
        _isMoving = true;
    }

    private void Death()
    {
        Destroy(gameObject);
    }
    private void OnTurnEnd()
    {
        _currentActionUnits = _startActionPoints;
        unitValuesUpdated?.Invoke();
    }

    public void PickupItem(GameObject item)
    {
        _inventory.AddItem(item.GetComponent<ItemScenePresenter>().Info);
        Destroy(item);
        _currentActionUnits -= INTERACTION_COST;

        _audioComponent.clip = _pickupClip;
        _audioComponent.Play();

        unitValuesUpdated?.Invoke();
    }

    public void ShootAtTarget(GameObject target)
    {
        target.GetComponent<UnitBase>().TakeDamage(_unitDamage);
        _currentActionUnits -= SHOT_COST;

        unitValuesUpdated?.Invoke();

        _audioComponent.Stop();
        _audioComponent.clip = _shotClip;
        _audioComponent.Play();
    }

    public void TakeDamage(int damage)
    {
        _unitHealth -= damage;
        unitValuesUpdated?.Invoke();

        if(_unitHealth <= 0)
        {
            Death();
        }
    }
}