using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Friends : MonoBehaviour
{
    [SerializeField] private UIFriend friend;
    [SerializeField] private Transform parent;

    private const string GetFriendUrl = "http://localhost:5107/GetAllFriends";
    private const string UserID = "UserID"; // Ключ, под которым хранится токен в PlayerPrefs


    private void OnEnable()
    {
        
    }
    
    private IEnumerator GetFriends()
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
    
  /*  private void OnSearchValueChanged(string searchText)
    {
        // Фильтруем список пользователей
        List<string> filteredUsers = allUsers
            .Where(user => user.ToLower().Contains(searchText.ToLower()))
            .ToList();

        // Обновляем отображение
        DisplayUsers(filteredUsers);
    }*/

    private void DisplayUsers(List<string> users)
    {
        // Удаляем старые элементы
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        // Создаем новые элементы для отображения
        foreach (string user in users)
        {
            GameObject userItem = Instantiate(friend.gameObject, parent);
            var friendUI = userItem.GetComponent<UIFriend>();
            friendUI.Set(user.ToLower());
             // Устанавливаем имя пользователя
        }
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
