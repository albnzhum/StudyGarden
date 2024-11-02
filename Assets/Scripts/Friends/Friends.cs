using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Friends : MonoBehaviour
{
    [SerializeField] private UIFriend friend;
    [SerializeField] private Transform parent;

    private const string GetFriendUrl = "https://localhost:44386/Friend/GetAllFriends";
    private const string UserID = "UserID"; // Ключ, под которым хранится токен в PlayerPrefs


    private void OnEnable()
    {
        
    }
    
    private IEnumerator GetTasksCoroutine()
    {
        var userID = PlayerPrefs.GetInt(UserID, 0);
        
        UnityWebRequest request = UnityWebRequest.Get(GetFriendUrl + "?userId=" + userID);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка при получении друзей: " + request.error);
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
    
    private void InstantiateTask(Task.Task taskToSet)
    {
        GameObject friendGO = Instantiate(friend.gameObject, parent);
        UIFriend task = friendGO.GetComponent<UIFriend>();
      //  task.Set(taskToSet);
    }
}

[Serializable]
public class Friend
{
    public int ID;

    public int UserID;
    public int FriendID;

    public bool IsAccepted;
}
