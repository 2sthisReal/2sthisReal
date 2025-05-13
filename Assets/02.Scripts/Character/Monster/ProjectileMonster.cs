using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMonster : Monster
{
    [Header("�߻�ü ����")]
    public GameObject projectilePrefab;  // �߻��� ������Ÿ�� ������
    public Transform firePoint;          // �߻� ��ġ
    private float attackCooldownTimer;

    // �߰�: �߻�ü �ӵ� ����
    public float projectileSpeed = 5f;
    // �߰�: ��������Ʈ ������ ����
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        // ��������Ʈ ������ ������Ʈ ��������
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Update()
    {
        if (!isAlive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // �÷��̾� ���� ���� ���
        Vector2 directionVector = (player.position - transform.position).normalized;

        // ��������Ʈ ���� ���� (�÷��̾� ��ġ�� ���� �¿� ����)
        if (spriteRenderer != null)
        {
            if (player.position.x - transform.position.x > 0)
                spriteRenderer.flipX = false;
            else if (player.position.x - transform.position.x < 0)
                spriteRenderer.flipX = true;
        }

        // �÷��̾� ���� (���� �������� �ָ� ���� ����)
        if (distance <= detectionRange && distance > attackRange)
        {
            Move(directionVector);
        }
        else if (distance <= attackRange)
        {
            // ���� ���� ���� ������ �̵� ����
            Move(Vector2.zero);
        }
        else
        {
            Move(Vector2.zero);
        }

        // ���� ��ٿ� ó��
        attackCooldownTimer -= Time.deltaTime;
        if (distance <= attackRange && attackCooldownTimer <= 0f)
        {
            Attack();
            attackCooldownTimer = 1f / attackSpeed;
        }
    }

    public override void Attack()
    {
        if (projectilePrefab == null || firePoint == null || player == null)
        {
            Debug.LogWarning($"{gameObject.name}�� projectilePrefab, firePoint �Ǵ� player�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �߻� ���� ��� (�÷��̾� ����)
        Vector2 directionVector = (player.position - firePoint.position).normalized;

        // ������Ÿ�� ���� �� �ʱ�ȭ
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // ������Ÿ�Ͽ� ���� �� �������� ����
        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(directionVector, attackDamage);
        }

        // ���� �ִϸ��̼� Ʈ����
        animator.SetTrigger("Attack");
    }

    // �߰�: ���Ÿ� ���ݿ� ���� Gizmo �ð�ȭ
    void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}