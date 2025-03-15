using UnityEngine;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour
{
    public Transform holdPosition; // Позиция удержания предмета
    public float throwForce = 10f; // Сила броска
    public Button throwButton; // Кнопка выкинуть

    private GameObject heldItem; // Текущий удерживаемый предмет
    private Rigidbody heldItemRb; // Rigidbody удерживаемого предмета

    void Start()
    {
        // Скрываем кнопку выкинуть в начале игры
        throwButton.gameObject.SetActive(false);

        // Назначаем метод ThrowItem на кнопку
        throwButton.onClick.AddListener(ThrowItem);
    }

    void Update()
    {
        // Обрабатываем касания
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Проверяем, что предмет можно подобрать
                if (hit.collider.CompareTag("Pickable"))
                {
                    PickUp(hit.collider.gameObject);
                }
            }
        }
    }

    // Подобрать предмет
    void PickUp(GameObject item)
    {
        if (heldItem != null) return; // Если уже что-то держим, не подбираем

        heldItem = item;
        heldItemRb = heldItem.GetComponent<Rigidbody>();

        // Отключаем физику у предмета
        heldItemRb.isKinematic = true;

        // Перемещаем предмет в позицию удержания
        heldItem.transform.SetParent(holdPosition);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;

        // Показываем кнопку выкинуть
        throwButton.gameObject.SetActive(true);
    }

    // Выкинуть предмет
    public void ThrowItem()
    {
        if (heldItem == null) return;

        // Включаем физику у предмета
        heldItemRb.isKinematic = false;
        heldItem.transform.SetParent(null);

        // Придаём импульс предмету вперёд (в направлении камеры)
        Vector3 throwDirection = Camera.main.transform.forward;
        heldItemRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        // Сбрасываем ссылки на предмет
        heldItem = null;
        heldItemRb = null;

        // Скрываем кнопку выкинуть
        throwButton.gameObject.SetActive(false);
    }
}