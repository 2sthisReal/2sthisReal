using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class PauseUI : BaseUI
    {
        [SerializeField] private Button breakButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button exitButton;
        protected override GameState GetUIState()
        {
            return GameState.Pause;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            breakButton.onClick.AddListener(
                () =>
                {
                    GameManager.Instance.ChangeState(GameState.InGame);
                });
            homeButton.onClick.AddListener(
                () =>
                {
                    GameManager.Instance.ChangeState(GameState.MainMenu);
                });
            exitButton.onClick.AddListener(
                () =>
                {
                    Application.Quit();
                });
        }
    }
}