public class HealItem : ItemBase
{
    protected override void ApplyStat(Player player, float value)
    {
        player.Heal(value);
    }
}
