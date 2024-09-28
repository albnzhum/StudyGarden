using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Task;
using UnityEngine;
using UnityEngine.Networking;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private UITask uiTask;

    private const string GetTasksUrl = "https://localhost:44386/Task/GetCurrentUserTasks";
    private const string TokenKey = "jwt_token"; // Ключ, под которым хранится токен в PlayerPrefs

    // Метод для получения всех задач пользователя

    private void Start()
    {
        GetUserTasks();
    }

    public void GetUserTasks()
    {
        StartCoroutine(GetTasksCoroutine());
    }

    // Coroutine для получения задач
    private IEnumerator GetTasksCoroutine()
    {
        string token = PlayerPrefs.GetString(TokenKey, null);

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Токен отсутствует. Необходима авторизация.");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(GetTasksUrl);
       // request.SetRequestHeader("Authorization", "Bearer " + token); // Добавляем токен в заголовок

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка при получении задач: " + request.error);
            yield break;
        }

        if (request.responseCode == 200)
        {
            Debug.Log(request.downloadHandler.text);
            // Преобразуем JSON ответ в список задач
            var tasks = JsonConvert.DeserializeObject<List<Task.Task>>(request.downloadHandler.text);

            // Обрабатываем задачи (например, отображаем их в UI)
            foreach (var task in tasks)
            {
                Debug.Log("Задача: " + task.Title + " Описание: " + task.Description);
            }
        }
        else
        {
            Debug.LogError($"Ошибка при получении задач. Код ответа: {request.responseCode}");
        }
    }

    private void InstantiateTask(Task.Task taskToSet)
    {
        GameObject taskGO = Instantiate(uiTask.gameObject, parent);
        UITask task = taskGO.GetComponent<UITask>();
        task.SetTask(taskToSet);
    }
}