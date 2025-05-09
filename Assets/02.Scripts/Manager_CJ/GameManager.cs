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

    public List<SkillData> SelectedSkills {  get; private set; } = new List<SkillData>();
    public Dictionary<EquipmentSlot, EquipmentData> EquippedSlots { get; private set; } = new();
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
    
    public bool HasSkill(string skillID)
    {
        return SelectedSkills.Exists(skill => skill.skillID == skillID);
    }

    #endregion

    #region 펫과 장비
    public void SetEquipment(EquipmentSlot slot, EquipmentData equipment)
    {
        EquippedSlots[slot] = equipment;
        Debug.Log($"[GameManager] Equipped {slot} : {equipment.equipmentName}");
    }

    // 특정 장비 반환
    public EquipmentData GetEquipment(EquipmentSlot slot)
    {
        return EquippedSlots.TryGetValue(slot, out var equipment) ? equipment : null;
    }

    public Dictionary<EquipmentSlot, EquipmentData> GetEquippedItems()
    {
        return new Dictionary<EquipmentSlot, EquipmentData>(EquippedSlots);
    }

    public void AutoAssignEquipment(EquipmentData equipment)
    {
        switch (equipment.equipmentType)
        {
            case EquipmentType.Weapon:
                SetEquipment(EquipmentSlot.Weapon, equipment);
                break;
            case EquipmentType.Armor:
                SetEquipment(EquipmentSlot.Armor, equipment);
                break;
            case EquipmentType.Accessory:
                if (!EquippedSlots.ContainsKey(EquipmentSlot.Accessory1))
                    SetEquipment(EquipmentSlot.Accessory1, equipment);
                else
                    SetEquipment(EquipmentSlot.Accessory2, equipment);
                break;
            case EquipmentType.Pet:
                if(!EquippedSlots.ContainsKey(EquipmentSlot.Pet1))
                    SetEquipment(EquipmentSlot.Pet1, equipment);
                else
                    SetEquipment(EquipmentSlot.Pet2, equipment);
                break;
        }
    }
    #endregion

    public void ResetPlayerSession()
    {
        ClearSelectedSkills();
        Debug.Log("[GameManager] Player session data has been reset");
    }
}
