using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour, IInteractable, IObject
{
    private const float DISTANCE_TO_OPEN = 1.8f;

    [Header ("Sprites")]
    [SerializeField] private Sprite _openedDoor;
    [SerializeField] private Sprite _closedDoor;

    [Space]
    [SerializeField] private bool _isOpened;

    private SpriteRenderer _sprite;
    private AudioSource _audio;

    public bool IsPassable => _isOpened;
    public bool CanShootThrough => _isOpened;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();

        _sprite = GetComponentInChildren<SpriteRenderer>();

        SetDoorSprite();
    }

    public void Interact(Unit unit)
    {
        if(Vector2.Distance(unit.transform.position, transform.position) < DISTANCE_TO_OPEN)
        {
            if (!_audio.isPlaying)
            {
                _isOpened = !_isOpened;
                ToggleDoor();
            }
        }
    }

    private void SetDoorSprite()
    {
        if (_isOpened)
            _sprite.sprite = _openedDoor;
        else
            _sprite.sprite = _closedDoor;
    }

    private void ToggleDoor()
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
    }
}
