using System.Collections;
using UnityEngine;

/// <summary>
/// BaseCharacter Ŭ������ ����, �÷��̾� �� ���� ĳ������
/// �⺻���� ����(�̵�, �ǰ�, ��� ��)�� �����ϴ� �߻� Ŭ�����Դϴ�.
/// 
/// �ַ�:
/// - ü��/���ݷ� �� �⺻ ����
/// - �̵� ó�� (���� ���)
/// - �ǰ�/��� ó��
/// 
/// �� �����մϴ�. Monster.cs ���� ���� ĳ���͵��� �� Ŭ������ ��ӹ޾� �����մϴ�.
/// </summary>
public abstract class BaseCharacter : MonoBehaviour
{
    [Header("�⺻ ����")]
    public string characterName;         // ĳ���� �̸� (ex: ������ �ü�)
    public int level = 1;                // ĳ���� ����
    public float maxHealth;              // �ִ� ü��
    public float currentHealth;          // ���� ü��
    public float moveSpeed;              // �̵� �ӵ�
    public float attackDamage;           // ���ݷ�
    public float attackSpeed;            // �ʴ� ���� Ƚ��
    public float attackRange;            // ���� �Ÿ�

    [Header("����")]
    public bool isAlive = true;          // ���� ����
    public bool isAttacking = false;     // ���� ���� ������ ����
    public bool isMoving = false;        // ���� �̵� ������ ����

    // ������Ʈ ĳ��
    protected Animator animator;         // �ִϸ��̼� �����
    protected Rigidbody2D rb;            // ���� �̵���

    /// <summary>
    /// ������Ʈ�� �������� ü���� �ʱ�ȭ�մϴ�.
    /// �ڽ� Ŭ�������� Ȯ���� �� �ֵ��� virtual ó��.
    /// </summary>
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;       // ������ �� ü���� �ִ�ġ�� ����
    }

    /// <summary>
    /// ���� ���͸� �޾� ĳ���͸� �̵���Ű�� �޼����Դϴ�.
    /// ������ ũ�Ⱑ 0�̸� �̵� ������ ó���˴ϴ�.
    /// </summary>
    /// <param name="direction">�̵��� ���� (��: (1, 0) �� ������)</param>
    public virtual void Move(Vector2 direction)
    {
        if (!isAlive) return;   // ���� ��� �̵� ����

        // ���� ���� ����ȭ �� �ӵ�/�ð� �ݿ��ؼ� �̵��� ���
        Vector2 movement = direction.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);  // ���� �̵�

        // �̵� ���� üũ�ؼ� �ִϸ��̼� �Ķ���� ����
        isMoving = direction.magnitude > 0;
        animator.SetBool("IsMoving", isMoving);

        // ���⿡ ���� ĳ���� �¿� ����
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
    }

    /// <summary>
    /// ���� ����. �߻� �޼����̹Ƿ� �ݵ�� �ڽ� Ŭ�������� �����ؾ� �մϴ�.
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// �������� ���� �� ȣ��Ǵ� �޼���.
    /// ���� ü���� ���ҽ�Ű�� ��� ���θ� üũ�մϴ�.
    /// </summary>
    /// <param name="damage">���� ������ ��</param>
    public virtual void TakeDamage(float damage)
    {
        if (!isAlive) return;   // �̹� �׾����� ����

        currentHealth -= damage;  // ü�� ����

        if (currentHealth <= 0)
        {
            Die();  // ü�� 0 ������ ��� ��� ó��
        }
        else
        {
            animator.SetTrigger("Hit");  // �ǰ� �ִϸ��̼�
        }
    }

    /// <summary>
    /// ��� ó�� �޼���.
    /// - isAlive �÷��׸� ����
    /// - ��� �ִϸ��̼� ����
    /// - �浹 ���� �� ���� �ùķ��̼� ��Ȱ��ȭ
    /// </summary>
    protected virtual void Die()
    {
        isAlive = false;
        animator.SetTrigger("Die");

        // �浹 ��Ȱ��ȭ (�ٸ� ������Ʈ�� �� �̻� �������� ����)
        GetComponent<Collider2D>().enabled = false;

        // Rigidbody �ùķ��̼� �� (�߷� � �� �̻� �� ����)
        rb.simulated = false;
    }
}
