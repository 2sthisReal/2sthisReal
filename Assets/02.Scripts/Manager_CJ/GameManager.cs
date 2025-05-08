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
