using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour, IInteractable
{
    private const float INTERACTION_DISTANCE = 1.8f;
    [SerializeField] private Sprite _openedDoor;
    [SerializeField] private Sprite _closedDoor;

    private SpriteRenderer _sprite;

    private AudioSource _audio;
    private Object _object;

    private bool _isOpened = false;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _object = GetComponent<Object>();

        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void Interact(Unit unit)
    {
        if(Vector2.Distance(unit.transform.position, transform.position) < INTERACTION_DISTANCE)
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        if (!_audio.isPlaying)
        {
            _isOpened = !_isOpened;
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
