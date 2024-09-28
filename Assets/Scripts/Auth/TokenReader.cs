using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class TokenReader : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onJWTTokenGet;
    [SerializeField] private VoidEventChannelSO gameSceneLoader;

    public UnityAction CheckTokenEvent = default;

    private void OnEnable()
    {
        CheckTokenEvent += StartButton;
    }

    private void OnDisable()
    {
        CheckTokenEvent -= StartButton;
    }

    public void StartButton()
    {
        StartCoroutine(CheckToken());
    }

    private const string AuthCheckUrl = "https://localhost:44386/Auth/CheckToken";
    private const string TokenKey = "jwt_token";  

    private IEnumerator CheckToken()
    {
        yield return StartCoroutine(CheckTokenValidity((isValid, message) =>
        {
            if (isValid)
            {
                Debug.Log("Пользователь авторизован: " + message);
                gameSceneLoader.RaiseEvent();
            }
            else
            {
                Debug.LogWarning("Пользователь не авторизован: " + message);
                onJWTTokenGet.RaiseEvent();
            }
        }));
    }
    
    public IEnumerator CheckTokenValidity(System.Action<bool, string> callback)
    {
        string token = PlayerPrefs.GetString(TokenKey, null);

        if (string.IsNullOrEmpty(token))
        {
            callback(false, "Токен отсутствует");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(AuthCheckUrl);
       request.SetRequestHeader("token", token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            callback(false, $"Ошибка сети: {request.error}");
            yield break;
        }

        if (request.responseCode == 200) 
        {
            callback(true, "Токен действителен");
        }
        else if (request.responseCode == 401)
        {
            callback(false, "Токен недействителен или истек");
        }
        else
        {
            callback(false, $"Неизвестная ошибка: {request.responseCode}");
        }
    }
}
