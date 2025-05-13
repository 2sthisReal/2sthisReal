using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : Monster
{
    protected override void Update()
    {
        base.Update();
        // ������ OnTriggerEnter2D������ ó��
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
                Debug.Log($"{characterName}�� ���� �������� {attackDamage} �������� ����");
            }
        }
    }
}
