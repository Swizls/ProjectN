using System;
using UnityEngine;

public class Cover : MonoBehaviour, IObject
{
    [Range(0, 1)][SerializeField] private float _accuracyReduce;
    [SerializeField] private bool _isPassable;
    [SerializeField] private bool _CanShootThrough;

    public float AccuracyReduce => _accuracyReduce;
    public bool IsPassable => _isPassable;
    public bool CanShootThrough => _CanShootThrough;
}
