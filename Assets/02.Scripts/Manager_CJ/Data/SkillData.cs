using UnityEngine;
using System;

[Serializable]
public class SkillData
{
    public string skillID;
    public string skillName;
    public float damage;
    public float cooldown;
    public GameObject skillPrefabPath;
    public Sprite iconPath;
    public SkillType skillType;
}

public enum SkillType
{
    Active,
    Passive
}
