public class MultiShot : SkillBase
{
    protected override void ApplyActiveSkill(Player player)
    {
        base.ApplyActiveSkill(player);
        player.ToggleMultiShot();
    }
}
