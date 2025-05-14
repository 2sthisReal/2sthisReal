using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillManager : MonoBehaviour
{
    private SkillDatabase skillDatabase;
    private readonly List<SkillConfig> selectedSkills = new();
    [SerializeField] private Player player;

    private void Awake()
    {
        skillDatabase = GetComponentInChildren<SkillDatabase>();
    }

    public void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        skillDatabase.LoadSkills();
    }

    public void AddSkill(SkillConfig skill)
    {
        selectedSkills.Add(skill);
        ApplySelectSkill(skill);

        Debug.Log($"[GameManager] Skill selected: {skill.skillName}");

    }

    // 선택한 스킬 리스트 반환
    public List<SkillConfig> GetAllSelectedSkills()
    {
        return new List<SkillConfig>(selectedSkills);
    }

    public void Clear()
    {
        selectedSkills.Clear();
        Debug.Log("[SkillManager] All selected skills cleared.");
    }

    /// <summary>
    /// Get Random 3 Skills
    /// </summary>
    /// <returns></returns>
    public List<SkillConfig> GetRandomSkills()
    {
        List<SkillConfig> skills = skillDatabase.GetSkills();
        var randomSkills = skills.OrderBy(x => Random.value).Take(3).ToList();

        return randomSkills;
    }

    // 선택한 스킬 적용
    private void ApplySelectSkill(SkillConfig skill)
    {
        SkillBase sb = Instantiate(skill.skillBase);
        sb.Init(skill);
        sb.ApplySkill(player);
        Destroy(sb.gameObject);

        if (skill.skillType == SkillType.Active)
        {
            skillDatabase.RemoveActiveSkill(skill);
        }
    }

    public void ResetSelectedSkills()
    {
        selectedSkills.Clear();
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
}
