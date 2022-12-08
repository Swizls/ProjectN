using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private const int CAM_MIN_SIZE = 2;
    private const int CAM_MAX_SIZE = 10;

    [SerializeField] private float _camSpeed = 10f;
    [SerializeField] private int _camScrollSpeed = 100;

    private void Update()
    {
        if (EndTurnHandler.isPlayerTurn)
            CamMovement();
        else
            FollowToEnemyUnits();
    }
    private void CamMovement()
    {
        transform.position = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * _camSpeed * Time.deltaTime,
                                        transform.position.y + Input.GetAxis("Vertical") * _camSpeed * Time.deltaTime, -10);

        //Увеличиваеться масштаб
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && Camera.main.orthographicSize < CAM_MAX_SIZE)
        {
            Camera.main.orthographicSize += _camScrollSpeed * Time.deltaTime;
            _camSpeed += 1f;
        }
        //Уменьшаеться масштаб
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f && Camera.main.orthographicSize > CAM_MIN_SIZE)
        {
            Camera.main.orthographicSize -= _camScrollSpeed * Time.deltaTime;
            _camSpeed -= 1f;
        }

        if (Camera.main.orthographicSize < CAM_MIN_SIZE)
        {
            Camera.main.orthographicSize = CAM_MIN_SIZE;
            _camSpeed = 3.5f;
        }
        else if (Camera.main.orthographicSize > CAM_MAX_SIZE)
        {
            Camera.main.orthographicSize = CAM_MAX_SIZE;
            _camSpeed = 15f;
        }
    }
    private void FollowToEnemyUnits()
    {
        Vector3 unitPosition = EnemyUnitControl.Instance.CurrentSelectedUnit.transform.position;

        transform.position = new(unitPosition.x, unitPosition.y, -10);
    }
}
