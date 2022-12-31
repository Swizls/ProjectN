using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour, IObject
{
    [SerializeField] private bool _isPassable;
    [SerializeField] private bool _canShootThorugh;

    public bool IsPassable => _isPassable;
    public bool CanShootThrough => _canShootThorugh;
}
