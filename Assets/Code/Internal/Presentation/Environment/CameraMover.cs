using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Start()
    {
        Vector3 currentRotation = transform.eulerAngles;
        yRotation = currentRotation.y;
        xRotation = currentRotation.x;

        if (xRotation > 180) xRotation -= 360;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yRotation -= mouseX;
            xRotation += mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}