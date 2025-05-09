using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene 
{
    public class HomeUI : BaseUI
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button exitButton;

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            startButton.onClick.AddListener(OnClickStartButton);
            exitButton.onClick.AddListener(OnClickExitButton);
        }

        public void OnClickStartButton()
        {
            GameManager.Instance.ChangeState(GameState.InGame);
            UIManager.instance.ChangeState(GameState.InGame);
        }

        public void OnClickExitButton()
        {
            Application.Quit();
        }

        protected override GameState GetUIState()
        {
            return GameState.MainMenu;
        }
    }
}