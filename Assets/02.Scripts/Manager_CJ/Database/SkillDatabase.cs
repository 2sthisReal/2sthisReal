using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    [SerializeField]
    public List<SkillConfig> Skills {  get; private set; }

    private void Awake()
    {
        LoadSkills();
    }

    private void LoadSkills()
    {
        Skills = Resources.LoadAll<SkillConfig>("Assets/Skills").ToList();
    }

    public List<SkillConfig> GetSkills()
    {
        return Skills;
    }
}
