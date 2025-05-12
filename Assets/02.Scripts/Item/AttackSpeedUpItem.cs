public class AttackSpeedUpItem : ItemBase
{
    protected override void ApplyStat(BaseCharacter player, float value)
    {
        player.attackSpeed += value;
    }
}
