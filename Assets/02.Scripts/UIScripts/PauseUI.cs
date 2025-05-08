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
        protected override UIState GetUIState()
        {
            return UIState.Pause;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            breakButton.onClick.AddListener(OnClickBreakButton);
            homeButton.onClick.AddListener(OnClickHomeButton);
            exitButton.onClick.AddListener(OnClickExitButton);
        }

        public void OnClickBreakButton()
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