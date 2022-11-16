using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private AudioClip _walkClip;

    private SpriteRenderer _sprite;
    private AudioSource _audio;

    private bool _isMoving = false;
    private bool _isAudioPlayed = false;

    public bool IsMoving => _isMoving;

    private void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _audio = GetComponent<AudioSource>();
    }

    private IEnumerator Move(List<Vector3> path)
    {
        const float MIN_DISTANCE = 0.07f;
        int _currentPathIndex = 0;
        _isMoving = true;

        if (!_isAudioPlayed)
        {
            _audio.clip = _walkClip;
            _audio.Play();
            _isAudioPlayed = true;
        }

        while (_isMoving)
        {
            if (path != null && path.Count != 0 && _isMoving)
            {
                if (Vector3.Distance(transform.position, path[path.Count - 1]) > MIN_DISTANCE)
                {
                    if (Vector3.Distance(transform.position, path[_currentPathIndex]) > MIN_DISTANCE)
                    {
                        Vector3 moveDirerction = (path[_currentPathIndex] - transform.position).normalized;
                        transform.position = transform.position + moveDirerction * _speed * Time.deltaTime;

                        if (Mathf.Round(moveDirerction.x) != 1)
                            _sprite.flipX = true;
                        else
                            _sprite.flipX = false;
                    }
                    else
                    {
                        _currentPathIndex++;
                    }
                    if (_currentPathIndex >= path.Count)
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
            yield return new WaitForEndOfFrame();
        }
    }
    private void StopMoving()
    {
        _isMoving = false;
        _audio.Stop();
        _isAudioPlayed = false;
    }
    public void StartMove(List<Vector3> path)
    {
        StartCoroutine(Move(path));
        _isMoving = true;
    }
}
