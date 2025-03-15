using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float sensitivity = 0.5f; // Чувствительность вращения
    public Joystick joystick; // Ссылка на джойстик

    private int joystickFingerId = -1; // ID пальца, который касается джойстика
    private Vector2 touchStartPosition;

    void Update()
    {
        // Обрабатываем все активные касания
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            // Если это новое касание, проверяем, началось ли оно на джойстике
            if (touch.phase == TouchPhase.Began)
            {
                if (IsTouchOnJoystick(touch.position))
                {
                    // Запоминаем ID пальца, который касается джойстика
                    joystickFingerId = touch.fingerId;
                }
            }

            // Если это движение пальца, который не касается джойстика, обрабатываем поворот камеры
            if (touch.fingerId != joystickFingerId && touch.phase == TouchPhase.Moved)
            {
                // Вычисляем разницу между текущим и начальным положением касания
                Vector2 touchDelta = touch.deltaPosition;

                // Вращение по оси Y (горизонтальное)
                float mouseX = touchDelta.x * sensitivity;
                transform.parent.Rotate(Vector3.up * mouseX);

                // Вращение по оси X (вертикальное)
                float mouseY = touchDelta.y * sensitivity;
                float xRotation = transform.localEulerAngles.x - mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Ограничиваем угол

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }

            // Если палец, который касался джойстика, отпущен, сбрасываем его ID
            if (touch.fingerId == joystickFingerId && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                joystickFingerId = -1;
            }
        }
    }

    // Проверка, находится ли касание в области джойстика
    private bool IsTouchOnJoystick(Vector2 touchPosition)
    {
        if (joystick == null) return false;

        // Получаем RectTransform джойстика
        RectTransform joystickRect = joystick.GetComponent<RectTransform>();
        Vector2 joystickPosition = joystickRect.position;
        Vector2 joystickSize = joystickRect.sizeDelta;

        // Проверяем, находится ли касание в пределах джойстика
        return touchPosition.x >= joystickPosition.x - joystickSize.x / 2 &&
               touchPosition.x <= joystickPosition.x + joystickSize.x / 2 &&
               touchPosition.y >= joystickPosition.y - joystickSize.y / 2 &&
               touchPosition.y <= joystickPosition.y + joystickSize.y / 2;
    }
}