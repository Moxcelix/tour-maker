using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private bool lockCursorWhenRotating = true;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool isRotating = false;

    private void Start()
    {
        Vector3 currentRotation = transform.eulerAngles;
        yRotation = currentRotation.y;
        xRotation = currentRotation.x;

        if (xRotation > 180) xRotation -= 360;
    }

    private void Update()
    {
        HandleMouseRotation();
    }

    private void HandleMouseRotation()
    {
        if (Input.GetMouseButtonDown(0) && CanStartRotation())
        {
            StartRotation();
        }

        if (Input.GetMouseButton(0) && isRotating)
        {
            RotateCamera();
        }

        if (Input.GetMouseButtonUp(0) && isRotating)
        {
            StopRotation();
        }
    }

    private bool CanStartRotation()
    {
        return !EventSystem.current.IsPointerOverGameObject();
    }

    private void StartRotation()
    {
        isRotating = true;

        if (lockCursorWhenRotating)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation -= mouseX;
        xRotation += mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void StopRotation()
    {
        isRotating = false;

        if (lockCursorWhenRotating)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void EnableRotation(bool enable)
    {
        if (!enable && isRotating)
        {
            StopRotation();
        }
        enabled = enable;
    }

    public void SetSensitivity(float sensitivity)
    {
        mouseSensitivity = Mathf.Clamp(sensitivity, 10f, 500f);
    }
}