using UnityEngine;
using System;
using TMPro;

[System.Serializable]
public class SkillData
{
    public string skillID;
    public string skillName;
    public string description;
    public float damage;
    public float cooldown;
    public string skillPrefabPath;
    public string skillIconPath;
    public int bonusProjectileCount;
    public SkillType skillType;
    public ElementType element;
    public float elementBonusDamage;
}