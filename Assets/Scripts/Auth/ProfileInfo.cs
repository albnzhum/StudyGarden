using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ProfileInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _username;
    
    private void OnEnable()
    {
       // StartCoroutine(GetTasksCoroutine());
    }
    
    private IEnumerator GetTasksCoroutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(EndpointMapper.GetPlantsByPlantTypeIDUrl );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка при получении профиля: " + request.error);
            yield break;
        }

        if (request.responseCode == 200)
        {
            Debug.Log(request.downloadHandler.text);
            // Преобразуем JSON ответ в список задач
           /* var tasks = JsonConvert.DeserializeObject<List<Task.Task>>(request.downloadHandler.text);

            foreach (var task in tasks)
            {
                InstantiateTask(task);
            }*/
        }
        else
        {
            Debug.LogError($"Ошибка при получении профиля. Код ответа: {request.responseCode}");
        }
    }
}
