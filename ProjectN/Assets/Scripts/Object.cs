using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField] private bool _isPassable;

    public bool IsPassable => _isPassable;
}
