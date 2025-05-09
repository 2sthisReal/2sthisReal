using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class StageClearUI : BaseUI
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private Button mainMenuButton;

        protected override GameState GetUIState()
        {
            return GameState.StageClear;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            nextButton.onClick.AddListener(
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