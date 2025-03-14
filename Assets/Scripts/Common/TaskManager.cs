using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private UITask uiTask;

    private const string UserID = "UserID"; // Ключ, под которым хранится токен в PlayerPrefs
    
    [SerializeField] private TaskVoidEventChannelSO onTaskCreated;
    
    private void OnEnable()
    {
        onTaskCreated.OnEventRaised += InstantiateTask;
    }

    private void OnDisable()
    {
        onTaskCreated.OnEventRaised -= InstantiateTask;
    }

    private void Start()
    {
        GetUserTasks();
    }

    public void GetUserTasks()
    {
        StartCoroutine(GetTasksCoroutine());
    }

    private IEnumerator GetTasksCoroutine()
    {
        var userID = PlayerPrefs.GetInt(UserID, 0);
        Debug.Log(userID);

        UnityWebRequest request = UnityWebRequest.Get(EndpointMapper.GetAllTasks + userID);

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

            foreach (var task in tasks)
            {
                InstantiateTask(task);
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