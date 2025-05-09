using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("�⺻ ����")]  // Inspector â���� �׷�ȭ�Ͽ� ǥ��
    public string characterName;   // ĳ������ �̸�
    public int level = 1;          // ĳ������ ����, �⺻�� 1
    public float maxHealth;        // �ִ� ü��
    public float currentHealth;    // ���� ü��
    public float moveSpeed;        // �̵� �ӵ�
    public float attackDamage;     // ���ݷ�
    public float attackSpeed;      // ���� �ӵ�(�ʴ� ���� Ƚ��)
    public float attackRange;      // ���� ����

    [Header("����")]  // ĳ������ ���� ���� ���� ����
    public bool isAlive = true;    // ���� ����, �⺻�� true
    public bool isAttacking = false; // ���� ������ ����
    public bool isMoving = false;  // �̵� ������ ����

    // �ڽ� Ŭ�������� ���� ������ ������Ʈ ����
    protected Animator animator;    // �ִϸ��̼� ��� ���� Animator ������Ʈ
    protected Rigidbody2D rb;      // ���� �̵��� ���� Rigidbody2D ������Ʈ

    // �ʱ�ȭ �޼���, virtual�� �����Ͽ� �ڽ� Ŭ�������� Ȯ�� ����
    protected virtual void Awake()
    {
        // �ʿ��� ������Ʈ ���� ��������
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // ���� �� ���� ü���� �ִ� ü������ ����
        currentHealth = maxHealth;
    }

    // ĳ���� �̵� ó�� �޼���
    public virtual void Move(Vector2 direction)
    {
        // ��� ���¸� �̵����� ����
        if (!isAlive) return;

        // ����ȭ�� ���� ���Ϳ� �̵� �ӵ��� �ð��� ���Ͽ� �̵��� ���
        Vector2 movement = direction.normalized * moveSpeed * Time.deltaTime;
        // Rigidbody2D�� ����Ͽ� ���������� ��ġ �̵�
        rb.MovePosition(rb.position + movement);

        // ���� ������ ũ�Ⱑ 0���� ũ�� �̵� ���� ���·� ����
        isMoving = direction.magnitude > 0;

        if (isMoving)
        {
            // �̵� ���̸� �ִϸ��̼� �Ķ���� ����
            animator.SetBool("IsMoving", true);

            // ĳ���� �¿� ���� ���� (x�� ����� ������, ������ ����)
            if (direction.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
            }
        }
        else
        {
            // �̵����� ������ �ִϸ��̼� �Ķ���� ����
            animator.SetBool("IsMoving", false);
        }
    }

    // ���� �޼��� - �߻� �޼���� �����Ͽ� �ڽ� Ŭ�������� �ݵ�� �����ϵ��� ��
    public abstract void Attack();

    // ������ ó�� �޼���
    public virtual void TakeDamage(float damage)
    {
        // ��� ���¸� �������� ���� ����
        if (!isAlive) return;

        // ���� ü�¿��� ��������ŭ ����
        currentHealth -= damage;

        // ������ ����Ʈ �Ǵ� UI ǥ�� ���� ���⼭ ó��

        // ü���� 0 ���ϸ� ��� ó��
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // �ǰ� �ִϸ��̼� Ʈ����
            animator.SetTrigger("Hit");
        }
    }

    // ��� ó�� �޼���
    protected virtual void Die()
    {
        // ��� ���·� ����
        isAlive = false;
        isAttacking = false;
        isMoving = false;

        // ��� �ִϸ��̼� Ʈ����
        animator.SetTrigger("Die");

        // ������ �浹 ��Ȱ��ȭ
        GetComponent<Collider2D>().enabled = false;
        // ���� �ùķ��̼� ��Ȱ��ȭ
        rb.simulated = false;
    }
}
