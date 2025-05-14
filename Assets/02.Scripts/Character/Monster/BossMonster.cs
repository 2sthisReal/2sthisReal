using UnityEngine;

public class BossMonster : Monster
{
    [Header("���� Ư�� �ɷ�")]
    public float healThreshold = 0.2f;     // ü�� ȸ�� �Ӱ��� (�ִ� ü���� %)
    public float healAmount = 0.3f;        // ȸ���� (�ִ� ü���� %)
    public float specialAttackThreshold = 0.5f;  // Ư�� ���� �ߵ� �Ӱ���

    private bool hasHealed = false;    // ü�� ȸ�� üũ
    private bool hasUsedSpecialAttack = false;  // Ư�� ���� ��� ����

    protected override void Update()
    {
        if (!isAlive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // �÷��̾� ����
        if (distance <= detectionRange && distance > attackRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);
        }
        else if (distance <= attackRange)
        {
            // ���� ���� ���� ������ ����
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

        // Ư�� �ɷ� �ߵ� üũ
        CheckSpecialAbilities();
    }

    private void CheckSpecialAbilities()
    {
        // ü�� ȸ�� üũ
        if (!hasHealed && currentHealth <= maxHealth * healThreshold)
        {
            Heal();
            hasHealed = true;
        }

        // Ư�� ���� üũ (��: ü���� 50% ������ ��)
        if (!hasUsedSpecialAttack && currentHealth <= maxHealth * specialAttackThreshold)
        {
            SpecialAttack();
            hasUsedSpecialAttack = true;
        }
    }

    public override void Attack()
    {
        // �⺻ ���� ����
        Debug.Log($"{characterName}�� ������ �����մϴ�.");

        // ���� ���� ����
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

        // ����Ʈ �� �α�
        Debug.Log($"{characterName}�� ü���� ȸ���߽��ϴ�. ���� ü��: {currentHealth}/{maxHealth}");

        // ȸ�� ����Ʈ�� �ִϸ��̼� �߰� (����)
        animator.SetTrigger("Heal");
    }

    private void SpecialAttack()
    {
        Debug.Log($"{characterName}�� Ư�� ������ ����մϴ�!");

        // Ư�� ���� ���� ���� (��: ���� ������, ��ȯ ��)
        animator.SetTrigger("SpecialAttack");

        // Ư�� ���� ���� (���� ������)
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