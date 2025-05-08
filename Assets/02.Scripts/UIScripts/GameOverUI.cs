using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class GameOverUI : BaseUI
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button exitButton;
        protected override UIState GetUIState()
        {
            return UIState.GameOver;
        }
        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            restartButton.onClick.AddListener(OnClickRestartButton);
            homeButton.onClick.AddListener(OnClickHomeButton);
            exitButton.onClick.AddListener(OnClickExitButton);
        }

        public void OnClickRestartButton()
        {
            GameManager.instance.StartGame();
        }

        public void OnClickHomeButton()
        {
            GameManager.instance.GoHome();
        }

        public void OnClickExitButton()
        {
            Application.Quit();
        }
    }
}
