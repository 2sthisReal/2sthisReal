using UnityEngine;

/// <summary>
/// Monster Ŭ������ BaseCharacter�� ��ӹ޾� ���� ������ �ൿ�� �����մϴ�.
/// - �÷��̾ �����ؼ� ����
/// - ���� �Ÿ� �̳��� ������ ���� (����/���Ÿ� ��� ����)
/// - MonsterData�� ������� ����/������ ����
/// </summary>
public class Monster : BaseCharacter
{
    [Header("���� �� ���� ����")]
    public float detectionRange = 15f;         // �÷��̾ Ž���ϴ� ����
    public GameObject projectilePrefab;        // �߻�ü ������ (���Ÿ� ���ݿ�)
    public Transform firePoint;                // �߻�ü �߻� ��ġ

    private Transform player;                  // ������ �÷��̾� ������Ʈ
    private MonsterData monsterData;           // �� ������ ������
    private float attackCooldown;              // ���� ��Ÿ��

    public string monsterId { get; private set; }  // �� ������ ���� ID

    /// <summary>
    /// ���� �� MonsterSpawner���� ID�� ���޹޾� �ʱ�ȭ�� �� ȣ��
    /// </summary>
    public void Initialize(string monsterId)
    {
        this.monsterId = monsterId;
    }

    private void Start()
    {
        // �÷��̾� ã�� (�±׷� Ž��)
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // ���� ������ �ҷ�����
        monsterData = MonsterManager.Instance.GetMonsterData(monsterId);
        if (monsterData == null)
        {
            Debug.LogError($"MonsterData�� ã�� �� �����ϴ�: {monsterId}");
            return;
        }

        // ������ ������� ���� �ʱ�ȭ
        characterName = monsterData.monsterName;
        maxHealth = monsterData.maxHealth;
        currentHealth = maxHealth;
        moveSpeed = monsterData.moveSpeed;
        attackDamage = monsterData.attackDamage;
        attackSpeed = monsterData.attackSpeed;
        attackRange = monsterData.attackRange;
    }

    private void Update()
    {
        if (!isAlive || player == null) return;

        // �÷��̾���� �Ÿ� ���
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            // �÷��̾ ���� �̵�
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);

            // ���� ��ٿ� üũ
            attackCooldown -= Time.deltaTime;
            if (distance <= attackRange && attackCooldown <= 0)
            {
                Attack();
            }
        }
        else
        {
            // Ž�� ���� ���̸� ����
            Move(Vector2.zero);
        }
    }

    /// <summary>
    /// ���� ���� (MonsterData�� attackPattern�� ���� �ٸ��� ����)
    /// </summary>
    public override void Attack()
    {
        isAttacking = true;
        attackCooldown = 1f / attackSpeed;  // ��Ÿ�� ����

        if (monsterData.attackPattern == AttackPattern.Ranged)
        {
            // ���Ÿ� ����: �߻�ü ����
            if (projectilePrefab != null && firePoint != null)
            {
                // �߻�ü �ν��Ͻ�ȭ
                GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

                // ���� ��� �� �߻�ü �ʱ�ȭ
                Vector2 dir = (player.position - firePoint.position).normalized;
                proj.GetComponent<Projectile>().Initialize(dir, attackDamage);
            }
            animator.SetTrigger("Attack");
        }
        else if (monsterData.attackPattern == AttackPattern.Melee)
        {
            // ���� ����: �ִϸ��̼Ǹ� ���� (�������� �浹 �� ó��)
            animator.SetTrigger("Attack");
            Debug.Log($"{characterName} ���� ���� ����!");
        }
        // ���߿� Charge ���� �߰� ���ϵ� ���⿡ ���� ����
    }
}
