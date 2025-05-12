using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private BaseUI pauseUI;
        [SerializeField] private BaseUI skillSelectUI;

        protected override GameState GetUIState()
        {
            return GameState.InGame;
        }

        protected override void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "InGameScene")
            {
                pauseUI.gameObject.SetActive(false);
                skillSelectUI.gameObject.SetActive(false);
            }
        }

        protected override void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            pauseUI.Init(uiManager);
            skillSelectUI.Init(uiManager); 
            pauseButton.onClick.AddListener(
                () => 
                {
                    pauseUI.gameObject.SetActive(true);
                    skillSelectUI.gameObject.SetActive(false);
                    //GameManager.Instance.ChangeState(GameState.Pause); 
                });
            tempSkillSelectButton.onClick.AddListener(
                () =>
                {
                    pauseUI.gameObject.SetActive(false);
                    skillSelectUI.gameObject.SetActive(true);
                    //GameManager.Instance.ChangeState(GameState.SkillSelect);
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
        }
    }
}