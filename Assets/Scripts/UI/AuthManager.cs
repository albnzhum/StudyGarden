using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onJWTTokenGet;

    private void Awake()
    {
        onJWTTokenGet.OnEventRaised += Show;
    }

    private void OnDisable()
    {
        onJWTTokenGet.OnEventRaised -= Show;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
