using System;
using System.Collections;
using System.Collections.Generic;
using Task;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreateTask : MonoBehaviour
{
    [SerializeField] private TMP_InputField titleText;
    [SerializeField] private TMP_InputField descriptionText;
    [SerializeField] private TMP_Dropdown category;
    [SerializeField] private TMP_Dropdown plant;
    [SerializeField] private TMP_Dropdown priority;
    
    private const string AddTaskUrl = "https://localhost:44386/Task/AddTask";  // Ваш URL для добавления задачи
    private const string TokenKey = "jwt_token";

    public void OnSubmitTask()
    {
        // Собираем данные из UI
        var newTask = new Task.Task
        {
            Title = titleText.text,
            Description = descriptionText.text,
            CategoryID = category.value,  // Предполагаем, что категории задаются через индекс
            PlantID = plant.value,  // Предполагаем, что категории задаются через индекс
            Priority = (Priority)priority.value,  // Преобразуем индекс Dropdown в enum Priority
            Status = Status.NotStarted,  // По умолчанию задача создается со статусом "Не начата"
        };

        // Отправляем задачу на сервер
        StartCoroutine(AddTaskCoroutine(newTask));
    }

    private IEnumerator AddTaskCoroutine(Task.Task newTask)
    {
        string token = PlayerPrefs.GetString(TokenKey, null);

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Токен отсутствует. Необходима авторизация.");
            yield break;
        }

        // Преобразуем задачу в JSON
        string jsonTask = JsonUtility.ToJson(newTask);

        // Создаем POST-запрос
        UnityWebRequest request = new UnityWebRequest(AddTaskUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonTask);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка при добавлении задачи: " + request.error);
        }
        else if (request.responseCode == 200)
        {
            Debug.Log("Задача успешно добавлена.");
        }
        else
        {
            Debug.LogError("Ошибка: " + request.responseCode);
        }
    }
}
