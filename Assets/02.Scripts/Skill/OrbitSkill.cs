using UnityEngine;

public class OrbitSkill : SkillBase
{
    [SerializeField] GameObject orbitSword;
    [SerializeField] float damageScale;
    protected override void ApplyActiveSkill(Player player)
    {
        GameObject obj = Instantiate(orbitSword, player.transform);
        
        for(int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).GetComponent<OrbitingSword>().Init(player.transform, player, damageScale);
        }
    }
}
