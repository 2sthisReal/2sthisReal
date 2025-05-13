using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Skill/Skill")]
public class SkillConfig : ScriptableObject
{
    public Sprite skillIcon;
    public string skillName;
    public SkillType skillType;
    public float skillValue;
    [TextArea(3, 5)]
    public string skillDescription;
    public SkillBase skillBase;
}