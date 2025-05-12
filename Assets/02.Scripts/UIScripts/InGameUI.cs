using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class InGameUI : BaseUI
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button tempGameOverButton;
        [SerializeField] private Button tempVictoryButton;
        [SerializeField] private Button tempStageClearButton;
        [SerializeField] private Button tempSkillSelectButton;

        protected override GameState GetUIState()
        {
            return GameState.InGame;
        }
        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            pauseButton.onClick.AddListener(
                () => 
                { 
                    GameManager.Instance.ChangeState(GameState.Pause); 
                });
            tempGameOverButton.onClick.AddListener(
                () => 
                { 
                    GameManager.Instance.ChangeState(GameState.GameOver); 
                });
            tempVictoryButton.onClick.AddListener(
                () => 
                { 
                    GameManager.Instance.ChangeState(GameState.Victory); 
                });
            tempStageClearButton.onClick.AddListener(
                () => 
                { 
                    GameManager.Instance.ChangeState(GameState.StageClear); 
                });
            tempSkillSelectButton.onClick.AddListener(
                () =>
                {
                    GameManager.Instance.ChangeState(GameState.SkillSelect);
                });
        }
    }
}