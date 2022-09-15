using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovementScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
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