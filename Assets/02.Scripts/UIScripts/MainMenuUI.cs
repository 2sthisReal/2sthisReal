using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene 
{
    public class MainMenuUI : BaseUI
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button preparingButton;
        [SerializeField] private Button exitButton;
        protected override GameState GetUIState()
        {
            return GameState.MainMenu;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            startButton.onClick.AddListener(
                () => 
                { 
                    GameManager.Instance.ChangeState(GameState.InGame); 
                });
            preparingButton.onClick.AddListener(
                () =>
                {
                    Application.Quit();
                    GameManager.Instance.ChangeState(GameState.Preparing);
                });
            exitButton.onClick.AddListener(
                () => 
                {
                    Application.Quit();
                });
        }
    }
}