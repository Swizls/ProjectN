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
    public bool TryExecute(IAction action)
    {
        return action.TryExecute(gameObject.GetComponent<Unit>(), ref _currentActionUnits);
    }
}
