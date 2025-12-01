using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        // Получаем начальный поворот камеры
        Vector3 currentRotation = transform.eulerAngles;
        yRotation = currentRotation.y;
        xRotation = currentRotation.x;

        // Корректируем угол для корректного ограничения
        if (xRotation > 180) xRotation -= 360;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Получаем движение мыши
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Применяем вращение
            yRotation -= mouseX;
            xRotation += mouseY; // Инвертируем вертикальное движение

            // Ограничиваем вертикальный поворот
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Применяем поворот
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

            // Блокируем курсор
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // Разблокируем курсор когда не вращаем
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}