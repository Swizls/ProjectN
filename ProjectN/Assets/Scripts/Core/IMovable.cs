using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable 
{
    public bool IsMoving { get; }

    public void Move();
}
