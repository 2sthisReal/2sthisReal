using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    [SerializeField]
    public List<SkillConfig> Skills {  get; private set; }

    public void LoadSkills()
    {
        Skills = Resources.LoadAll<SkillConfig>("Skills").ToList();
        Debug.Log($"Loaded Skills: {Skills.Count}");
    }

    public List<SkillConfig> GetSkills()
    {
        return Skills;
    }

    public void RemoveActiveSkill(SkillConfig selectSkill)
    {
        if(Skills.Contains(selectSkill))
            Skills.Remove(selectSkill);
    }
}
