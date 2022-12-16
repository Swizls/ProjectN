using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(UnitHealth))]
[RequireComponent(typeof(UnitMovement))]
[RequireComponent(typeof(UnitInventory))]
public class Unit : MonoBehaviour
{
    private UnitInventory _inventory;
    private UnitHealth _health;
    private UnitMovement _movement;
    private UnitActions _actions;

    private Tilemap _tilemap;

    public Action unitValuesUpdated;
    public Action<Unit> unitDied;

    public UnitMovement Movement => _movement;
    public UnitInventory Inventory => _inventory;
    public UnitHealth Health => _health;
    public UnitActions Actions => _actions;

    public Tilemap Tilemap => _tilemap;


    private void Start()
    {
        _health = GetComponent<UnitHealth>();
        _movement = GetComponent<UnitMovement>();
        _inventory = GetComponent<UnitInventory>();
        _actions = GetComponent<UnitActions>();

        _tilemap = FindObjectOfType<Tilemap>();

        _health.damageTaken += unitValuesUpdated;

        unitValuesUpdated?.Invoke();
    }

    private void OnEnable()
    {
        if(_health != null)
            _health.damageTaken += unitValuesUpdated;
        EndTurnHandler.playerTurn += OnTurnEnd;
    }

    private void OnDisable()
    {
        EndTurnHandler.playerTurn -= OnTurnEnd;
        _health.damageTaken -= unitValuesUpdated;
    }
    private void OnDestroy()
    {
        unitDied?.Invoke(this);
    }

    private void OnTurnEnd()
    {
        unitValuesUpdated?.Invoke();
    }
}