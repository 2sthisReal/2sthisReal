using System.Collections;
using UnityEngine;

public class MeleeMonster : Monster
{
    // 디버깅을 위한 변수 추가
    [SerializeField] private bool showDebugInfo = true;
    private bool isAttacking = false;

    protected override void Update()
    {
        if (!isAlive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 플레이어 방향
        Vector2 directionVector = (player.position - transform.position).normalized;

        // 애니메이터 상태 전환 처리
        if (distance > attackRange && isAttacking)
        {
            isAttacking = false;
            animator.SetTrigger("IsMove"); 
        }

        // 기존 움직임 처리
        if (distance <= detectionRange && distance > attackRange)
        {
            Move(directionVector);
        }
        else if (distance <= attackRange)
        {
            Move(Vector2.zero);
        }
        else
        {
            Move(Vector2.zero);
        }

        // 공격 처리
        attackCooldown -= Time.deltaTime;
        if (distance <= attackRange && attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / attackSpeed;
        }
    }


    public override void Attack()
    {
        if (isAttacking) return; // 공격 중이면 중복 공격 방지

        isAttacking = true;

        Debug.Log($"{characterName}가 근접 공격을 시작합니다.");

        if (animator != null)
        {
            animator.SetTrigger("IsAttack");
            StartCoroutine(DelayedMeleeDamage());
        }
        else
        {
            Debug.LogError("Animator is null!");
            StartCoroutine(DelayedMeleeDamage());
        }
    }

    private IEnumerator DelayedMeleeDamage()
    {
        yield return new WaitForSeconds(0.3f);

        float currentDistance = Vector2.Distance(transform.position, player.position);
        Debug.Log($"Damage check - Distance: {currentDistance}, Attack Range: {attackRange}");

        if (currentDistance <= attackRange)
        {
            BaseCharacter playerCharacter = player.GetComponent<BaseCharacter>();
            if (playerCharacter != null)
            {
                playerCharacter.TakeDamage(attackDamage);
                Debug.Log($"{characterName}가 {attackDamage} 데미지를 적용했습니다.");
            }
            else
            {
                Debug.LogError("BaseCharacter 컴포넌트없음!");
            }
        }
    }

    // 충돌 감지를 위한 OnCollisionEnter2D 추가
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
            Debug.Log($"{characterName}가 플레이어와 충돌했습니다!");
        }
    }

    // 트리거 충돌 감지를 위한 OnTriggerEnter2D 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{characterName}가 플레이어 트리거와 충돌했습니다!");
        }
    }
}