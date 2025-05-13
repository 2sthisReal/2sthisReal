using UnityEngine;

public class PassiveSkill : SkillBase
{
    [SerializeField] PassiveType type;
    protected override void ApplyPassiveSkill(Player player)
    {
        switch (type)
        {
            case PassiveType.Attack:
                player.attackDamage += skill.skillValue;
                break;

            case PassiveType.AttackSpeed:
                player.attackSpeed += skill.skillValue;
                break;

            case PassiveType.MaxHp:
                player.maxHealth += skill.skillValue;
                break;
        }
    }
}
