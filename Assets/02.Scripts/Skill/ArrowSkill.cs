using UnityEngine;

public class ArrowSkill : SkillBase
{
    [SerializeField] ArrowSkillType arrowType;
    protected override void ApplyActiveSkill(Player player)
    {
        Weapon_Bow weapon = player.CurrentWeapon.GetComponent<Weapon_Bow>();

        if (weapon)
        {
            weapon.arrowSkills.Add(arrowType);
        }
    }
}
