using System;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO gameSceneLoader;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            gameSceneLoader.OnEventRaised += GameSceneLoad;
        }

        private void OnDisable()
        {
            gameSceneLoader.OnEventRaised -= GameSceneLoad;
        }

        private void GameSceneLoad()
        {
            SceneManager.UnloadSceneAsync("Launch");
            SceneManager.LoadScene("Game");
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
    }
}