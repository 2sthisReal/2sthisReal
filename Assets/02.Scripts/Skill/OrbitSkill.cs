using UnityEngine;

public class OrbitSkill : SkillBase
{
    [SerializeField] GameObject orbitSword;
    [SerializeField] float damageScale;
    protected override void ApplyActiveSkill(Player player)
    {
        for(int i = 0; i < 2; i++)
        {
            float angleOffset = (Mathf.PI * 2f / 2) * i;
            GameObject sword = Instantiate(orbitSword);
            sword.GetComponent<OrbitingSword>().Init(player.transform, player, damageScale, angleOffset);
        }
    }
}
