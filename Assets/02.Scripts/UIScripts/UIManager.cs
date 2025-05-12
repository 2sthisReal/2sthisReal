using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SWScene
{

    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get; private set; }
        List<BaseUI> uiList = new List<BaseUI>();
        private GameState currentState;
        public Dictionary<string, GameState> sceneStateBinding { get; private set; } = new Dictionary<string, GameState>()
        {
            { "MainMenuScene", GameState.MainMenu },
            { "InGameScene", GameState.InGame },
            { "VictoryScene", GameState.Victory },
            { "PreparingScene", GameState.Preparing },
            { "StageClearScene", GameState.StageClear },
            { "PauseScene", GameState.Pause},
            { "GameOverScene", GameState.GameOver },
            { "SkillSelectScene", GameState.SkillSelect }
        };


        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            RegisterUI(GetComponentInChildren<MainMenuUI>(true));
            RegisterUI(GetComponentInChildren<InGameUI>(true));
            RegisterUI(GetComponentInChildren<VictoryUI>(true));
            RegisterUI(GetComponentInChildren<PreparingUI>(true));
            RegisterUI(GetComponentInChildren<StageClearUI>(true));
            RegisterUI(GetComponentInChildren<PauseUI>(true));
            RegisterUI(GetComponentInChildren<GameOverUI>(true));
            RegisterUI(GetComponentInChildren<SkillSelectUI>(true));
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void RegisterUI(BaseUI UI)
        {
            uiList.Add(UI);
            UI.Init(this);
        }

        private void Start()
        {
            GameManager.Instance.ChangeState(GameState.MainMenu);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ChangeState(sceneStateBinding[scene.name]);
        }

        public void ChangeState(GameState state)
        {
            currentState = state;
            foreach(BaseUI ui in uiList)
            {
                ui.SetActive(currentState);
            }
        }
    }
}