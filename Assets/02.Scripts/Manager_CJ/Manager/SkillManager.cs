using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private readonly List<SkillData> selectedSkills = new();

    public void AddSkill(SkillData skill)
    {
        if (!selectedSkills.Contains(skill))
        {
            selectedSkills.Add(skill);
            Debug.Log($"[GameManager] Skill selected: {skill.skillName}");
        }
    }

    public List<SkillData> GetAll()
    {
        return new List<SkillData>(selectedSkills);
    }

    public bool Has(string skillID)
    {
        return selectedSkills.Exists(skill => skill.skillID == skillID);
    }

    public void Clear()
    {
        selectedSkills.Clear();
        Debug.Log("[SkillManager] All selected skills cleared.");
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
    // 스킬 효과가 많아지면 이 부분만 확장하거나 분리
}
