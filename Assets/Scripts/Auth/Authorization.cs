using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using API.Contracts.Authorization;
using Events;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Authorization : MonoBehaviour
{
    [SerializeField] private TMP_InputField loginInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TokenReader tokenReader;
    
    public StringEventChannelSO AuthUserEvent;

    private void OnEnable()
    {
        AuthUserEvent.OnEventRaised += Login;
    }

    private void OnDisable()
    {
        AuthUserEvent.OnEventRaised -= Login;
    }

    public void Login()
    {
        StartCoroutine(SendLoginRequest(loginInput.text, passwordInput.text));
    }

    public void Login(string username, string password)
    {
        StartCoroutine(SendLoginRequest(username, password));
    }

    private IEnumerator SendLoginRequest(string username, string password)
    {
        UserModel requestData = new UserModel
        {
            login = username,
            password = password
        };

        string json = JsonUtility.ToJson(requestData);
        Debug.Log(json);

        UnityWebRequest request = new UnityWebRequest(EndpointMapper.Login, "POST");
        
        Debug.Log(EndpointMapper.Login);
        
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Отправляем запрос на сервер
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Получаем токен из ответа
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            var responseData = JsonUtility.FromJson<LoginResponse>(jsonResponse);

            // Сохраняем токен в PlayerPrefs (в дальнейшем можно использовать другой механизм)
            PlayerPrefs.SetString("jwt_token", responseData.token);
            Debug.Log(responseData.token);
            PlayerPrefs.SetInt("UserID", responseData.id);

            Debug.Log("Авторизация успешна, токен сохранен.");
            tokenReader.CheckTokenEvent.Invoke();
        }
        else
        {
            Debug.LogError("Ошибка авторизации: " + request.error);
        }
    }
}