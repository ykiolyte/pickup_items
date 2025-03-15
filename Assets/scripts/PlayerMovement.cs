using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Скорость перемещения
    public Joystick joystick; // Ссылка на джойстик
    public Transform cameraTransform; // Ссылка на трансформ камеры

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Получаем ввод от джойстика
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Вычисляем направление движения относительно камеры
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Преобразуем направление в мировые координаты, учитывая направление камеры
        direction = cameraTransform.TransformDirection(direction);
        direction.y = 0; // Обнуляем вертикальную составляющую, чтобы персонаж не летел

        // Перемещаем персонажа
        if (direction.magnitude >= 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
}