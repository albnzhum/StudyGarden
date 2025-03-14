using System.Collections;
using System.Collections.Generic;
using System.Text;
using API.Contracts.Authorization;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Registration : MonoBehaviour
{
    [SerializeField] private TMP_InputField loginInput;
    [SerializeField] private TMP_InputField passwordInput;

    public StringEventChannelSO AuthUserEvent;

    public void RegisterUser()
    {
        string username = loginInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return;
        }

        StartCoroutine(RegisterCoroutine(username, password));
        
        
    }

    private IEnumerator RegisterCoroutine(string username, string password)
    {
        // Создаем объект для отправки данных
        UserModel userData = new UserModel
        {
            login = username,
            password = password
        };

        // Преобразуем объект в JSON
        string jsonData = JsonUtility.ToJson(userData);

        // Создаем запрос
        UnityWebRequest request = new UnityWebRequest(EndpointMapper.Register, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Отправляем запрос на сервер
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Ошибка: {request.error}");
        }
        else
        {
            Debug.Log("Регистрация успешна!");
            AuthUserEvent.RaiseEvent(username, password);
        }
    }
}
