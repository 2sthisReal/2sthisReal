using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    #region 상태 변환
    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        if(newState == GameState.MainMenu || newState == GameState.InGame)
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
    }
    #endregion

    #region 몬스터
    // 스테이지 시작 시 적 수 등록해주면 됩니다. (Stage쪽)
    public void RegisterEnemies(int count)
    {
        remainingEnemies = count;
        Debug.Log($"[GameManager] Registered {count} enemies");
    }

    // NotifyEnemyKilled()를 Die()에서 호출해주면 됩니다. (Monster쪽)
    public void NotifyEnemyKilled()
    {
        if (CurrentState != GameState.InGame) return;

        remainingEnemies = Mathf.Max(remainingEnemies - 1, 0);
        Debug.Log($"[GameManager] Enemy killed. Remaining: {remainingEnemies}");

        if(remainingEnemies <= 0)
        {
            Debug.Log("[GameManager] All enemies defeated. Stage Clear.");
            ChangeState(GameState.StageClear);
        }
    }
    #endregion

    public void ResetPlayerSession()
    {
        Skills.Clear();
        Debug.Log("[GameManager] Player session data has been reset");
    }
}
