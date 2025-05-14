public class AttackSpeedUpItem : ItemBase
{
    protected override void ApplyStat(Player player, float value)
    {
        player.attackSpeed += value;
    }
}
