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
        protected override UIState GetUIState()
        {
            return UIState.Game;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            pauseButton.onClick.AddListener(OnClickPauseButton);
            tempGameOverButton.onClick.AddListener(OnClickGameOverButton);
        }

        public void OnClickPauseButton()
        {
            GameManager.instance.PauseGame();
        }
        public void OnClickGameOverButton()
        {
            GameManager.instance.GameOver();
        }
    }
}