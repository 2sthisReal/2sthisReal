public class MultiShot : SkillBase
{
    protected override void ApplyActiveSkill(BaseCharacter player)
    {
        base.ApplyActiveSkill(player);
        player.GetComponent<Player>().ToggleMultiShot();
    }
}
