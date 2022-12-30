using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour, IInteractable
{
    private const float INTERACTION_DISTANCE = 1.8f;

    [Header ("Sprites")]
    [SerializeField] private Sprite _openedDoor;
    [SerializeField] private Sprite _closedDoor;

    [Space]
    [SerializeField] private bool _isOpened;

    private SpriteRenderer _sprite;

    private AudioSource _audio;
    private Object _object;

    public bool IsOpened => _isOpened;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _object = GetComponent<Object>();

        _sprite = GetComponentInChildren<SpriteRenderer>();

        SetDoor();
    }

    public void Interact(Unit unit)
    {
        if(Vector2.Distance(unit.transform.position, transform.position) < INTERACTION_DISTANCE)
        {
            _isOpened = !_isOpened;
            ToggleDoor();
        }
    }

    private void SetDoor()
    {
        if (_isOpened)
            _sprite.sprite = _openedDoor;
        else
            _sprite.sprite = _closedDoor;

        _object.IsPassable = _isOpened;
    }

    private void ToggleDoor()
    {
        if (!_audio.isPlaying)
        {
            if (_isOpened)
            {
                _sprite.sprite = _openedDoor;
            }
            else
            {
                _sprite.sprite = _closedDoor;
            }
            _audio.Play();
            _object.IsPassable = _isOpened;
        }
    }
}
