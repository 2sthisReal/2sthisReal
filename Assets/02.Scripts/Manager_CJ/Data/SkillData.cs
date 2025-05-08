using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Game Data/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public float damage;
    public float cooldown;
    public GameObject skillPrefab;
    public Sprite icon;
    public SkillType skillType;
}

public enum SkillType
{
    Active,
    Passive
}
