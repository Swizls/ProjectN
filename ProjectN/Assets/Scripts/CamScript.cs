using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CamScript : MonoBehaviour
{
    [SerializeField]
    private float camSpeed = 10f;
    [SerializeField]
    private int camScrollSpeed = 100;
    private int camMinSize = 2;
    private int camMaxSize = 10;

    [SerializeField]
    GridLayout gridLayout;

    void Update()
    {
        CamMovement();
    }
    private void CamMovement()
    {
        transform.position = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * camSpeed * Time.deltaTime,
            transform.position.y + Input.GetAxis("Vertical") * camSpeed * Time.deltaTime, -10);

        //Увеличиваеться масштаб
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && Camera.main.orthographicSize < camMaxSize)
        {
            Camera.main.orthographicSize += camScrollSpeed * Time.deltaTime;
            camSpeed += 1f;
        }
        //Уменьшаеться масштаб
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f && Camera.main.orthographicSize > camMinSize)
        {
            Camera.main.orthographicSize -= camScrollSpeed * Time.deltaTime;
            camSpeed -= 1f;
        }

        if (Camera.main.orthographicSize < camMinSize)
        {
            Camera.main.orthographicSize = camMinSize;
            camSpeed = 3.5f;
        }
        else if (Camera.main.orthographicSize > camMaxSize)
        {
            Camera.main.orthographicSize = camMaxSize;
            camSpeed = 15f;
        }
    }
}
