using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class GameUI : BaseUI
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button tempGameOverButton;
        protected override GameState GetUIState()
        {
            return GameState.InGame;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            pauseButton.onClick.AddListener(OnClickPauseButton);
            tempGameOverButton.onClick.AddListener(OnClickGameOverButton);
        }

        public void OnClickPauseButton()
        {
            GameManager.Instance.ChangeState(GameState.Pause);
            UIManager.instance.ChangeState(GameState.Pause);
        }
        public void OnClickGameOverButton()
        {
            GameManager.Instance.ChangeState(GameState.GameOver);
            UIManager.instance.ChangeState(GameState.GameOver);
        }
    }
}