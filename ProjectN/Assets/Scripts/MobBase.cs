using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBase : MonoBehaviour
{
    private int healthPoints = 100;
    protected float speed = 5f;

    private void Update()
    {
        Death();
    }
    protected virtual void UnitMovement() 
    {
    }
    void Death()
    {
        if(healthPoints <=0)
        {
            Destroy(gameObject);
        }
    }
}
