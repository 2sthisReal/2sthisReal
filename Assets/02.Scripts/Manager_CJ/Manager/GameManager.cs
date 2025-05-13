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

    #region ���� ��ȯ
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

    #region ����
    // �������� ���� �� �� �� ������ָ� �˴ϴ�. (Stage��)
    public void RegisterEnemies(int count)
    {
        remainingEnemies = count;
        Debug.Log($"[GameManager] Registered {count} enemies");
    }

    // NotifyEnemyKilled()�� Die()���� ȣ�����ָ� �˴ϴ�. (Monster��)
    public void NotifyEnemyKilled()
    {
        if (CurrentState != GameState.InGame) return;

        remainingEnemies = Mathf.Max(remainingEnemies - 1, 0);
        Debug.Log($"[GameManager] Enemy killed. Remaining: {remainingEnemies}");

        if(remainingEnemies <= 0)
        {
            Debug.Log("[GameManager] All enemies defeated. Stage Clear.");
            //ChangeState(GameState.StageClear);
            stageManager.StageClear();
        }
    }

    [ContextMenu("MoveToNextStage")]
    public void MoveToNextStage()
    {
        stageManager.StartStage();
    }
    #endregion

    public void ResetPlayerSession()
    {
        Skills.Clear();
        Debug.Log("[GameManager] Player session data has been reset");
    }
}
