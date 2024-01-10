using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    Vector3 lastMousePosition;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scrollWheel * Time.deltaTime * 500f;
        GetComponent<Camera>().orthographicSize  -= zoomAmount;
    }

}
