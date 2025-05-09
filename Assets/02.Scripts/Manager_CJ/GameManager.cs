using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Game State & Player Session
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    public List<SkillData> SelectedSkills {  get; private set; } = new List<SkillData>();
    public Dictionary<EquipmentType, EquipmentData> EquippedEquipments { get; private set; } = new();
    public List<PetData> SelectedPets { get; private set; } = new();
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

    private void SetState(GameState newState)
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

    #region 스킬
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

    // 스킬 정보 반환
    public List<SkillData> GetSelectedSkills()
    {
        return new List<SkillData>(SelectedSkills);
    }

    public void ClearSelectedSkills()
    {
        SelectedSkills.Clear();
        Debug.Log("[GameManager] All selected skills cleared.");
    }
    #endregion

    #region 장비
    public void SetEquipment(EquipmentType type, EquipmentData equipment)
    {
        EquippedEquipments[type] = equipment;
        Debug.Log($"[GameManager] Equipped {type} : {equipment.equipmentName}");
    }

    // 특정 장비 반환
    public EquipmentData GetEquipment(EquipmentType type)
    {
        return EquippedEquipments.TryGetValue(type, out var equipment) ? equipment : null;
    }

    public Dictionary<EquipmentType, EquipmentData> GetEquippedItems()
    {
        return new Dictionary<EquipmentType, EquipmentData>(EquippedEquipments);
    }
    #endregion

    #region 펫
    public void AddSelectedPet(PetData pet)
    {
        if(SelectedPets.Count < 2)
        {
            SelectedPets.Add(pet);
        }
    }

    // 복사본 반환
    public List<PetData> GetSelectedPets()
    {
        return new List<PetData>(SelectedPets);
    }
    #endregion

    public void ResetPlayerSession()
    {
        ClearSelectedSkills();
        Debug.Log("[GameManager] Player session data has been reset");
    }
}
