using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    public SkillData SelectedSkill {  get; private set; }
    public EquipmentData SelectedEquipment { get; private set; }

    private int remainingEnemies;

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

    private void SetState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

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

    public void SetSelectedSkill(SkillData skill)
    {
        SelectedSkill = skill;
        Debug.Log($"[GameManager] Skill selected: {skill.skillName}");
    }

    public void SetSelectedEquipment(EquipmentData equipment)
    {
        SelectedEquipment = equipment;
        Debug.Log($"[GameManager] Equipment selected: {equipment.equipmentName}");
    }
}
