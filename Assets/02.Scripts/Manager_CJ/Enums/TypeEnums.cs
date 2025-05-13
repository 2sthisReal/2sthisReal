using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon = 0,
    Armor = 1,
    Accessory = 2,
    Pet = 3
}

public enum SkillType
{
    Passive = 0,
    Active = 1
}

public enum PassiveType
{
    Attack = 0,
    AttackSpeed = 1,
    MaxHp = 2
}

public enum ArrowSkillType
{
    Pierce, // 관통
    WallReflect, // 벽 반사
    MonsterBounce, // 몬스터 튕기기
}
public enum ElementType
{
    None = -1,
    Fire = 0,
    Ice = 1,
    Lightning = 2,
    Poison = 3
}

public enum DamageType
{
    Physical = 0,
    Elemental = 1
}