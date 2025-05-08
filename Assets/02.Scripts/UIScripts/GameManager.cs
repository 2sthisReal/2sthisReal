using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SWScene
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance { get; private set; }
        [SerializeField] private UIManager uiManager;
        void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartGame()
        {
            uiManager.SetPlayGame();
        }

        public void BreakGame()
        {
            uiManager.SetPlayGame();
        }

        public void PauseGame()
        {
            uiManager.SetPause();
        }

        public void GoHome()
        {
            uiManager.SetHome();
        }

        public void GameOver()
        {
            uiManager.SetGameOver();
        }
           
    }

    public static class Logger
    {
        public static void Log(string log)
        {
#if UNITY_EDITOR
            Debug.Log(log);
#endif
        }
        public static void LogWarning(string log)
        {
#if UNITY_EDITOR
            Debug.LogWarning(log);
#endif
        }
        public static void LogError(string log)
        {
#if UNITY_EDITOR
            Debug.LogError(log);
#endif
        }
    }
}