using UnityEngine;

public class CamScript : MonoBehaviour
{
    private const int CAM_MIN_SIZE = 2;
    private const int CAM_MAX_SIZE = 10;

    [SerializeField] private float camSpeed = 10f;
    [SerializeField] private int camScrollSpeed = 100;

    void Update()
    {
        CamMovement();
    }
    private void CamMovement()
    {
        transform.position = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * camSpeed * Time.deltaTime,
            transform.position.y + Input.GetAxis("Vertical") * camSpeed * Time.deltaTime, -10);

        //Увеличиваеться масштаб
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && Camera.main.orthographicSize < CAM_MAX_SIZE)
        {
            Camera.main.orthographicSize += camScrollSpeed * Time.deltaTime;
            camSpeed += 1f;
        }
        //Уменьшаеться масштаб
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f && Camera.main.orthographicSize > CAM_MIN_SIZE)
        {
            Camera.main.orthographicSize -= camScrollSpeed * Time.deltaTime;
            camSpeed -= 1f;
        }

        if (Camera.main.orthographicSize < CAM_MIN_SIZE)
        {
            Camera.main.orthographicSize = CAM_MIN_SIZE;
            camSpeed = 3.5f;
        }
        else if (Camera.main.orthographicSize > CAM_MAX_SIZE)
        {
            Camera.main.orthographicSize = CAM_MAX_SIZE;
            camSpeed = 15f;
        }
    }
}
