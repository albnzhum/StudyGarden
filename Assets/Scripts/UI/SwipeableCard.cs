using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeableCard : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 initialPosition; // Начальная позиция карточки
    private float swipeThreshold = 100f; // Порог для определения смахивания
    private bool isDragging = false; // Флаг для отслеживания состояния перетаскивания

    [SerializeField] private Vector3 swipeUpTarget; // Целевая позиция при свайпе вверх
    [SerializeField] private Vector3 swipeDownTarget; // Целевая позиция при свайпе вниз

    private void Start()
    {
        initialPosition = GetComponent<RectTransform>().anchoredPosition; // Сохраняем стартовую позицию
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Перемещение карточки вместе с движением пальца
        Vector3 newPosition = GetComponent<RectTransform>().anchoredPosition;
        newPosition.y += eventData.delta.y; // Изменяем только y-координату
        GetComponent<RectTransform>().anchoredPosition = newPosition; // Применяем новую позицию

        isDragging = true; // Устанавливаем флаг перетаскивания
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false; // Сбрасываем флаг перетаскивания

        // Определение расстояния смахивания
        float distance = GetComponent<RectTransform>().anchoredPosition.y - initialPosition.y;

        if (Mathf.Abs(distance) > swipeThreshold)
        {
            // Если смахнули вниз
            if (distance < 0)
            {
                // Перемещаем карточку вниз к заданной позиции
                MoveToPosition(swipeDownTarget);
            }
            else
            {
                // Перемещаем карточку вверх к заданной позиции
                MoveToPosition(swipeUpTarget);
            }
        }
        else
        {
            // Если смахнули не достаточно, возвращаем карточку обратно
            MoveToPosition(initialPosition);
        }
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        // Анимация перемещения карточки к целевой позиции
        StartCoroutine(MoveTo(targetPosition));
    }

    private System.Collections.IEnumerator MoveTo(Vector3 targetPosition)
    {
        float duration = 0.3f; // Время анимации
        Vector3 startPosition = GetComponent<RectTransform>().anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null; // Ждем до следующего кадра
        }

        // Устанавливаем окончательную позицию
        GetComponent<RectTransform>().anchoredPosition = targetPosition;
    }
}
