using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Events;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Authorization : MonoBehaviour
{
    private const string AuthUrl = "https://localhost:44386/Auth/Login";
    private const string UserIDUrl = "https://localhost:44386/Auth/GetUserID";

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
        StartCoroutine(GetUserID(loginInput.text, passwordInput.text));
    }

    public void Login(string username, string password)
    {
        StartCoroutine(SendLoginRequest(username, password));
        StartCoroutine(GetUserID(username, password));
    }

    private IEnumerator GetUserID(string username, string password)
    {
        UserModel requestData = new UserModel
        {
            Username = username,
            Password = password
        };

        string json = JsonUtility.ToJson(requestData);

        UnityWebRequest request = new UnityWebRequest(UserIDUrl, "POST");
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
            var responseData = JsonConvert.DeserializeObject<UserID>(jsonResponse);
            Debug.Log(responseData.id);

            PlayerPrefs.SetInt("UserID", responseData.id);
        }
        else
        {
            Debug.LogError("Ошибка авторизации: " + request.error);
        }
    }

    private IEnumerator SendLoginRequest(string username, string password)
    {
        UserModel requestData = new UserModel
        {
            Username = username,
            Password = password
        };

        string json = JsonUtility.ToJson(requestData);
        Debug.Log(json);

        UnityWebRequest request = new UnityWebRequest(AuthUrl, "POST");
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
            var responseData = JsonUtility.FromJson<TokenResponse>(jsonResponse);

            // Сохраняем токен в PlayerPrefs (в дальнейшем можно использовать другой механизм)
            PlayerPrefs.SetString("jwt_token", responseData.token);

            Debug.Log("Авторизация успешна, токен сохранен.");
            tokenReader.CheckTokenEvent.Invoke();
        }
        else
        {
            Debug.LogError("Ошибка авторизации: " + request.error);
        }
    }
}

[Serializable]
public class UserModel
{
    public string Username;
    public string Password;
}

[Serializable]
public class UserID
{
    public int id;
}

[Serializable]
public class TokenResponse
{
    public string token;
}
