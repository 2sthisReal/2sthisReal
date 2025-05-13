using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : Monster
{
    protected override void Update()
    {
        base.Update();
        // 공격은 OnTriggerEnter2D에서만 처리
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive || monsterData.attackPattern != AttackPattern.Melee) return;

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
                Debug.Log($"{characterName}이 근접 공격으로 {attackDamage} 데미지를 입힘");
            }
        }
    }
}
