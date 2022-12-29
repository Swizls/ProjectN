using System;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [Range(0, 1)][SerializeField] private float _accuracyReduce;

    public float AccuracyReduce => _accuracyReduce;
}
