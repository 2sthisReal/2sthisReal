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
        [SerializeField] AudioClip loseClip;

        protected override GameState GetUIState()
        {
            return GameState.GameOver;
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
            exitButton.onClick.AddListener(
                () =>
                {
                    Application.Quit();
                });
        }

        protected override void OnEnable()
        {
            SoundManager.PlayClip(loseClip);
        }
    }
}
