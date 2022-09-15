using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitMovement : MobBase
{
    void Update()
    {
        MobMovement();
    }
    private void MobMovement()
    {
        bool isMoving = false;
        if (Input.GetMouseButton(1))
        {
            isMoving = true;
        }
        while (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 5 * Time.deltaTime);
            if (transform.position == Camera.main.ScreenToWorldPoint(Input.mousePosition)) isMoving = false;
        }
    }
}
