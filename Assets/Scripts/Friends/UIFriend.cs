using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFriend : MonoBehaviour
{
    [SerializeField] private TMP_Text friendName;

    public void Set(string username)
    {
        friendName.text = username;
    }
}
