using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SWScene
{
    public enum UIState
    {
        Home,
        Game,
        GameOver,
        Pause
    }

    public class UIManager : MonoBehaviour
    {
        HomeUI homeUI;
        GameUI gameUI;
        GameOverUI gameOverUI;
        PauseUI pauseUI;
        private UIState currentState;


        private void Awake()
        {
            homeUI = GetComponentInChildren<HomeUI>(true);
            homeUI.Init(this);
            gameUI = GetComponentInChildren<GameUI>(true);
            gameUI.Init(this);
            gameOverUI = GetComponentInChildren<GameOverUI>(true);
            gameOverUI.Init(this);
            pauseUI = GetComponentInChildren<PauseUI>(true);
            pauseUI.Init(this);

            ChangeState(UIState.Home);
        }

        public void SetPlayGame()
        {
            ChangeState(UIState.Game);
        }

        public void SetGameOver()
        {
            ChangeState(UIState.GameOver);
        }
        public void SetHome()
        {
            ChangeState(UIState.Home);
        }

        public void SetPause()
        {
            ChangeState(UIState.Pause);
        }

        public void ChangeState(UIState state)
        {
            currentState = state;
            homeUI.SetActive(currentState);
            gameUI.SetActive(currentState);
            gameOverUI.SetActive(currentState);
            pauseUI.SetActive(currentState);
        }
    }
}