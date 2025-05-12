using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillManager : MonoBehaviour
{
    private SkillDatabase skillDatabase;
    private readonly List<SkillConfig> selectedSkills = new();
    [SerializeField] private BaseCharacter player;
    public SkillConfig skillConfig;

    private void Awake()
    {
        skillDatabase = GetComponentInChildren<SkillDatabase>();
    }

    public void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<BaseCharacter>();
    }

    public void AddSkill(SkillConfig skill)
    {
        if (!selectedSkills.Contains(skill))
        {
            selectedSkills.Add(skill);
            Debug.Log($"[GameManager] Skill selected: {skill.skillName}");
        }
    }

    public List<SkillConfig> GetAll()
    {
        return new List<SkillConfig>(selectedSkills);
    }

    public bool Has(string skillName)
    {
        return selectedSkills.Exists(skill => skill.skillName == skillName);
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
        return skills.OrderBy(x => Random.value).Take(3).ToList();
    }

    public void ApplySelectSkill(SkillConfig skill)
    {
        SkillBase sb = Instantiate(skill.skillBase);
        sb.Init(skill);
        sb.ApplySkill(player);
        Destroy(sb.gameObject);
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
