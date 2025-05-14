using System;
using System.Collections;
using System.Collections.Generic;
using Jang;
using SWScene;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Game State & Player Session
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged;
    [SerializeField] private UIManager uiManager;

    public SkillManager Skills { get; private set; } = new();
    public EquipmentManager Equipment { get; private set; } = new();
    public PetManager Pets { get; private set; } = new();

    private bool isPaused = false;
    #endregion

    #region Stage Progress
    private int remainingEnemies;
    private StageManager stageManager;
    public SkillManager skillManager { get; private set; } = new();
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        stageManager = GetComponentInChildren<StageManager>();
        skillManager = GetComponentInChildren<SkillManager>();
    }

    #region Change & Set Game State
    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        SceneLoader.Instance.LoadSceneForState(newState);
        return;

        if (newState == GameState.MainMenu || newState == GameState.InGame)
        {
            SceneLoader.Instance.LoadSceneForState(newState);
            return;
        }

        SetState(newState);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);

        if(newState == GameState.GameOver || newState == GameState.Victory)
        {
            ResetPlayerSession();
        }

        if(newState == GameState.InGame)
        {
            stageManager.Init(this);
            skillManager.Init();
        }
    }
    #endregion

    #region Enemy Management
    public void RegisterEnemies(int count)
    {
        remainingEnemies = count;
    }

    public void NotifyEnemyKilled()
    {
        if (CurrentState != GameState.InGame) return;

        remainingEnemies = Mathf.Max(remainingEnemies - 1, 0);

        if(remainingEnemies <= 0)
        {
            //ChangeState(GameState.StageClear);
            stageManager.StageClear();
        }
    }
    #endregion

    #region Move Next Stage
    [ContextMenu("MoveToNextStage")]
    public void MoveToNextStage()
    {
        stageManager.StartStage();
    }
    #endregion

    #region Player
    public void ResetPlayerSession()
    {
        Skills.Clear();
    }
    #endregion

    #region Pause System
    public void PauseGame()
    {
        if (isPaused) return;

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }
    #endregion
}
