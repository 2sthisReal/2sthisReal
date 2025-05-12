using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class VictoryUI : BaseUI
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button homeButton;
        protected override GameState GetUIState()
        {
            return GameState.Victory;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            restartButton.onClick.AddListener(
                () =>
                {
                    GameManager.Instance.ChangeState(GameState.InGame);
                });
            homeButton.onClick.AddListener(
                () =>
                {
                    GameManager.Instance.ChangeState(GameState.MainMenu);
                });
        }
    }
}