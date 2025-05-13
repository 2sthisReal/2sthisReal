using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [SerializeField] protected SkillConfig skill;

    public virtual void Init(SkillConfig skill)
    {
        this.skill = skill;
    }

    public void ApplySkill(Player player)
    {
        switch (skill.skillType)
        {
            case SkillType.Passive:
                ApplyPassiveSkill(player);
                break;

            case SkillType.Active:
                ApplyActiveSkill(player);
                break;
        }
    }
    protected virtual void ApplyPassiveSkill(Player player)
    {

    }

    protected virtual void ApplyActiveSkill(Player player)
    {

    }
}
