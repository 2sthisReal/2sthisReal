using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class PreparingUI : BaseUI
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button mainMenuButton;
        protected override GameState GetUIState()
        {
            return GameState.Preparing;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            startButton.onClick.AddListener(
                () =>
                {
                    GameManager.Instance.ChangeState(GameState.InGame);
                });
            mainMenuButton.onClick.AddListener(
                () =>
                {
                    GameManager.Instance.ChangeState(GameState.MainMenu);
                });
        }
    }
}