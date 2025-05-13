using System.Collections;
using UnityEngine;

public class MeleeMonster : Monster
{
    protected override void Update()
    {
        // 근접 몬스터는 추적 범위 내에 들어왔을 때 근접으로 공격만 함
        if (!isAlive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);

            // 공격
            attackCooldown -= Time.deltaTime;
            if (distance <= attackRange && attackCooldown <= 0)
            {
                Attack();
                attackCooldown = 1f / attackSpeed;  // 공격 쿨다운 리셋
            }
        }
        else
        {
            Move(Vector2.zero);
        }
    }

    public override void Attack()
    {
        Debug.Log($"{characterName}가 근접 공격을 시작합니다.");
        animator.SetTrigger("Attack");
        StartCoroutine(DelayedMeleeDamage());
    }

    private IEnumerator DelayedMeleeDamage()
    {
        yield return new WaitForSeconds(0.3f); // 애니메이션 중간 타격 타이밍에 맞춤

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<BaseCharacter>()?.TakeDamage(attackDamage);
            Debug.Log($"{characterName}가 데미지를 적용했습니다.");
        }
    }
}