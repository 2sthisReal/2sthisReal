using UnityEngine;

public class BossMonster : Monster
{
    [Header("보스 특수 능력")]
    public float healThreshold = 0.2f;     // 체력 회복 임계점 (최대 체력의 %)
    public float healAmount = 0.3f;        // 회복량 (최대 체력의 %)
    public float specialAttackThreshold = 0.5f;  // 특수 공격 발동 임계점

    private bool hasHealed = false;    // 체력 회복 체크
    private bool hasUsedSpecialAttack = false;  // 특수 공격 사용 여부

    protected override void Update()
    {
        if (!isAlive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 플레이어 추적
        if (distance <= detectionRange && distance > attackRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);
        }
        else if (distance <= attackRange)
        {
            // 공격 범위 내에 들어오면 공격
            Move(Vector2.zero);
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                Attack();
                attackCooldown = 1f / attackSpeed;
            }
        }
        else
        {
            Move(Vector2.zero);
        }

        // 특수 능력 발동 체크
        CheckSpecialAbilities();
    }

    private void CheckSpecialAbilities()
    {
        // 체력 회복 체크
        if (!hasHealed && currentHealth <= maxHealth * healThreshold)
        {
            Heal();
            hasHealed = true;
        }

        // 특수 공격 체크 (예: 체력이 50% 이하일 때)
        if (!hasUsedSpecialAttack && currentHealth <= maxHealth * specialAttackThreshold)
        {
            SpecialAttack();
            hasUsedSpecialAttack = true;
        }
    }

    public override void Attack()
    {
        // 기본 공격 로직
        Debug.Log($"{characterName}가 공격을 시작합니다.");

        // 근접 공격 로직
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            player.GetComponent<BaseCharacter>()?.TakeDamage(attackDamage);
        }
    }

    private void Heal()
    {
        float healAmountValue = maxHealth * healAmount;
        currentHealth = Mathf.Min(currentHealth + healAmountValue, maxHealth);

        // 이펙트 및 로그
        Debug.Log($"{characterName}가 체력을 회복했습니다. 현재 체력: {currentHealth}/{maxHealth}");

        // 회복 이펙트나 애니메이션 추가 (예시)
        animator.SetTrigger("Heal");
    }

    private void SpecialAttack()
    {
        Debug.Log($"{characterName}가 특수 공격을 사용합니다!");

        // 특수 공격 로직 구현 (예: 광역 데미지, 소환 등)
        animator.SetTrigger("SpecialAttack");

        // 특수 공격 예시 (광역 데미지)
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange * 2);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                hitCollider.GetComponent<BaseCharacter>()?.TakeDamage(attackDamage * 2);
            }
        }
    }
}