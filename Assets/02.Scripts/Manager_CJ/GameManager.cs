using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    public List<SkillData> SelectedSkills {  get; private set; } = new List<SkillData>();
    public Dictionary<EquipmentType, EquipmentData> EquippedItems { get; private set; } = new();
    public List<PetData> SelectedPets { get; private set; } = new();

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

        if(newState == GameState.GameOver || newState == GameState.Victory)
        {
            ResetPlayerSession();
        }
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

    public void AddSelectedSkill(SkillData skill)
    {
        if (!SelectedSkills.Contains(skill))
        {
            SelectedSkills.Add(skill);
            Debug.Log($"[GameManager] Skill selected: {skill.skillName}");
        }
    }

    //public void ApplySelectedSkillsToPlayer(PlayerController player, List<PetController> pets)
    //{
    //    foreach(var skill in SelectedSkills)
    //    {
    //        if(skill.bonusProjectileCount > 0)
    //        {
    //            player.AddBonusProjectiles(skill.bonusProjectileCount);
    //        }

    //        if(skill.element != ElementType.None && skill.elementBonusDamage > 0)
    //        {
    //            player.SetElementalDamage(skill.element, skill.elementBonusDamage);
    //        }

    //        if(skill.skillID == "wingman")
    //        {
    //            foreach(var pet in pets)
    //            {
    //                pet.EnableProjectileBlocking();
    //            }
    //        }
    //    }
    //    Debug.Log("[GameManager] Selected skill effects applied.");
    //}

    public void ClearSelectedSkills()
    {
        SelectedSkills.Clear();
        Debug.Log("[GameManager] All selected skills cleared.");
    }

    public void SetEquipment(EquipmentType type, EquipmentData equipment)
    {
        EquippedItems[type] = equipment;
        Debug.Log($"[GameManager] Equipped {type} : {equipment.equipmentName}");
    }

    public EquipmentData GetEquipment(EquipmentType type)
    {
        return EquippedItems.TryGetValue(type, out var equipment) ? equipment : null;
    }

    public void AddSelectedPet(PetData pet)
    {
        if(SelectedPets.Count < 2)
        {
            SelectedPets.Add(pet);
        }
    }

    public void ResetPlayerSession()
    {
        ClearSelectedSkills();
        Debug.Log("[GameManager] Player session data has been reset");
    }
}
