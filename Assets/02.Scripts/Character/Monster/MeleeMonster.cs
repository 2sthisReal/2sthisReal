using System.Collections;
using UnityEngine;

public class MeleeMonster : Monster
{
    // ������� ���� ���� �߰�
    [SerializeField] private bool showDebugInfo = true;
    private bool isAttacking = false;
    private Vector2 directionVector;

    protected override void Update()
    {
        if (!isAlive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // �÷��̾� ����
        directionVector = (player.position - transform.position).normalized;

        // �ִϸ����� ���� ��ȯ ó��
        if (distance > attackRange && isAttacking)
        {
            isAttacking = false;
            animator.SetTrigger("IsMove"); 
        }

        // ���� ������ ó��
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

        // ���� ó��
        attackCooldown -= Time.deltaTime;
        if (distance <= attackRange && attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / attackSpeed;
        }
    }


    public override void Attack()
    {
        if (isAttacking) return; // ���� ���̸� �ߺ� ���� ����

        isAttacking = true;

        Debug.Log($"{characterName}�� ���� ������ �����մϴ�.");

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

    protected override void Die()
    {
        base.Die();
    }

    private IEnumerator DelayedMeleeDamage()
    {
        yield return new WaitForSeconds(0.3f);

        float currentDistance = Vector2.Distance(transform.position, player.position);
        Debug.Log($"Damage check - Distance: {currentDistance}, Attack Range: {attackRange}");

        if (currentDistance <= attackRange)
        {
            Player playerCharacter = player.GetComponent<Player>();
            if (playerCharacter != null)
            {
                playerCharacter.KnockbackPlayer(directionVector, 5f);
                playerCharacter.TakeDamage(attackDamage);
                Debug.Log($"{characterName}�� {attackDamage} �������� �����߽��ϴ�.");
            }
            else
            {
                Debug.LogError("BaseCharacter ������Ʈ����!");
            }
        }

        animator.SetBool("IsAttack", false);
    }

    // �浹 ������ ���� OnCollisionEnter2D �߰�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
            Debug.Log($"{characterName}�� �÷��̾�� �浹�߽��ϴ�!");
        }
    }

    // Ʈ���� �浹 ������ ���� OnTriggerEnter2D �߰�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{characterName}�� �÷��̾� Ʈ���ſ� �浹�߽��ϴ�!");
        }
    }
}