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

    #region ���� ��ȯ
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
            ChangeState(GameState.StageClear);
        }
    }
    #endregion

    #region ��ų
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

    // ��ų ���� ��ȯ
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

    #region ���
    public void SetEquipment(EquipmentType type, EquipmentData equipment)
    {
        EquippedEquipments[type] = equipment;
        Debug.Log($"[GameManager] Equipped {type} : {equipment.equipmentName}");
    }

    // Ư�� ��� ��ȯ
    public EquipmentData GetEquipment(EquipmentType type)
    {
        return EquippedEquipments.TryGetValue(type, out var equipment) ? equipment : null;
    }

    public Dictionary<EquipmentType, EquipmentData> GetEquippedItems()
    {
        return new Dictionary<EquipmentType, EquipmentData>(EquippedEquipments);
    }
    #endregion

    #region ��
    public void AddSelectedPet(PetData pet)
    {
        if(SelectedPets.Count < 2)
        {
            SelectedPets.Add(pet);
        }
    }

    // ���纻 ��ȯ
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
