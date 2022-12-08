using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UnitActions : MonoBehaviour
{
    private readonly int _startActionPoints = 20;
    private int _currentActionUnits;

    private AudioSource _audio;

    public AudioSource Audio => _audio;
    public int ActionUnits => _currentActionUnits;

    private void Start()
    {
        _currentActionUnits = _startActionPoints;

        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EndTurnHandler.playerTurn += OnTurnEnd;
    }

    private void OnDisable()
    {
        EndTurnHandler.playerTurn -= OnTurnEnd;
    }

    public bool TryExecute(IAction action)
    {
        bool result = action.TryExecute(gameObject.GetComponent<Unit>(), ref _currentActionUnits);
        if(result)
            PlayerUnitControl.Instance.CurrentSelectedUnit.unitValuesUpdated?.Invoke();
        return result;
    }

    private void OnTurnEnd() => _currentActionUnits = _startActionPoints;
}
