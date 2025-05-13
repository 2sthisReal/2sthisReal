using System.Collections;
using UnityEngine;

public class MeleeMonster : Monster
{
    protected override void Update()
    {
        // ���� ���ʹ� ���� ���� ���� ������ �� �������� ���ݸ� ��
        if (!isAlive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);

            // ����
            attackCooldown -= Time.deltaTime;
            if (distance <= attackRange && attackCooldown <= 0)
            {
                Attack();
                attackCooldown = 1f / attackSpeed;  // ���� ��ٿ� ����
            }
        }
        else
        {
            Move(Vector2.zero);
        }
    }

    public override void Attack()
    {
        Debug.Log($"{characterName}�� ���� ������ �����մϴ�.");
        animator.SetTrigger("Attack");
        StartCoroutine(DelayedMeleeDamage());
    }

    private IEnumerator DelayedMeleeDamage()
    {
        yield return new WaitForSeconds(0.3f); // �ִϸ��̼� �߰� Ÿ�� Ÿ�ֿ̹� ����

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<BaseCharacter>()?.TakeDamage(attackDamage);
            Debug.Log($"{characterName}�� �������� �����߽��ϴ�.");
        }
    }
}