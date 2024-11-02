using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Newtonsoft.Json;
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
    
    private const string AddTaskUrl = "https://localhost:44386/Task/AddNewTask";
    private const string LoadCategoryUrl = "https://localhost:44386/Category/GetCurrentUserCategories"; // Ваш URL для добавления задачи
    private const string LoadPlantsUrl = "https://localhost:44386/Plant/GetAllPlants";
    private const string UserID = "UserID";

    [SerializeField] private TaskVoidEventChannelSO onTaskCreated;

    private void OnEnable()
    {
        StartCoroutine(GetCurrentUserCategories());
        StartCoroutine(GetCurrentUserPlants());
    }

    public void OnSubmitTask()
    {
        // Собираем данные из UI
        var newTask = new Task.Task
        {
            Title = titleText.text,
            UserID = PlayerPrefs.GetInt(UserID, 0),
            Description = descriptionText.text,
            CategoryID = category.value+1,  // Предполагаем, что категории задаются через индекс
            PlantID = plant.value+1,  // Предполагаем, что категории задаются через индекс
            Priority = (Priority)priority.value,  // Преобразуем индекс Dropdown в enum Priority
            Status = Status.NotStarted,  // По умолчанию задача создается со статусом "Не начата"
        };

        StartCoroutine(AddTaskCoroutine(newTask));
        
        onTaskCreated.RaiseEvent(newTask);
    }

    private IEnumerator GetCurrentUserCategories()
    {
        var userID = PlayerPrefs.GetInt(UserID, 0);
        
        UnityWebRequest request = UnityWebRequest.Get(LoadCategoryUrl + "?userId=" + userID);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Ошибка сети: {request.error}");
            yield break;
        }

        if (request.responseCode == 200)
        {
            Debug.Log(request.downloadHandler.text);
            // Преобразуем JSON ответ в список задач
            var categories = JsonConvert.DeserializeObject<List<Category>>(request.downloadHandler.text);

            List<string> titles = new List<string>();
            
            foreach (var task in categories)
            {
                titles.Add(task.Title);
            }
            
            category.ClearOptions();
            category.AddOptions(titles);
        }
        else
        {
            Debug.LogError($"Ошибка при получении категорий. Код ответа: {request.responseCode}");
        }
    }

    private IEnumerator AddTaskCoroutine(Task.Task newTask)
    {
        var userID = PlayerPrefs.GetInt(UserID, 0);
        
        // Преобразуем задачу в JSON
        string jsonTask = JsonUtility.ToJson(newTask);

        // Создаем POST-запрос
        UnityWebRequest request = new UnityWebRequest(AddTaskUrl + "?userId=" + userID, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonTask);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

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

    private IEnumerator GetCurrentUserPlants()
    {
        UnityWebRequest request = UnityWebRequest.Get(LoadPlantsUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Ошибка сети: {request.error}");
            yield break;
        }

        if (request.responseCode == 200)
        {
            Debug.Log(request.downloadHandler.text);
            // Преобразуем JSON ответ в список задач
            var plants = JsonConvert.DeserializeObject<List<Plant>>(request.downloadHandler.text);

            List<string> titles = new List<string>();
            
            foreach (var plant in plants)
            {
                titles.Add(plant.Name);
            }
            
            plant.ClearOptions();
            plant.AddOptions(titles);
        }
        else
        {
            Debug.LogError($"Ошибка при получении категорий. Код ответа: {request.responseCode}");
        }
    }

    [Serializable]
    public class Category
    {
        public int ID;

        public int UserID;

        public int PlantTypeID;

        public string Title;
    }

    [Serializable]
    public class Plant
    {
        public int ID;
        public string Name;

        public int PlantTypeID;

        public bool IsUnlocked;
    }
}
