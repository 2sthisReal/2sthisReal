using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SWScene
{

    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get; private set; }
        HomeUI homeUI;
        GameUI gameUI;
        GameOverUI gameOverUI;
        PauseUI pauseUI;
        private GameState currentState;


        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            homeUI = GetComponentInChildren<HomeUI>(true);
            homeUI.Init(this);
            gameUI = GetComponentInChildren<GameUI>(true);
            gameUI.Init(this);
            gameOverUI = GetComponentInChildren<GameOverUI>(true);
            gameOverUI.Init(this);
            pauseUI = GetComponentInChildren<PauseUI>(true);
            pauseUI.Init(this);
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        private void Start()
        {
            SetHome();
        }

        public void SetPlayGame()
        {
            GameManager.Instance.ChangeState(GameState.InGame);
        }

        public void SetGameOver()
        {
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
        public void SetHome()
        {
            GameManager.Instance.ChangeState(GameState.MainMenu);
        }

        public void SetPause()
        {
            GameManager.Instance.ChangeState(GameState.Pause);
        }

        public void ChangeState(GameState state)
        {
            currentState = state;
            homeUI.SetActive(currentState);
            gameUI.SetActive(currentState);
            gameOverUI.SetActive(currentState);
            pauseUI.SetActive(currentState);
        }
    }
}