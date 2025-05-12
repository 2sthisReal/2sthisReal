using UnityEngine;

/// <summary>
/// Monster Ŭ������ BaseCharacter�� ��ӹ޾� ������ �⺻ �ൿ�� �����մϴ�.
/// - �÷��̾ Ž���ϰ� �̵�
/// - ���Ÿ� ���͸� ���� �޼��� ȣ��
/// - ���� ���ʹ� �浹 �� ������ ����
/// </summary>
public class Monster : BaseCharacter
{
    [Header("���� �� ���� ����")]
    public float detectionRange = 15f;           // �÷��̾� Ž�� ����

    private Transform player;                    // ���� ��� (�÷��̾�)
    private MonsterData monsterData;             // ���� �ɷ�ġ �� ���� Ÿ��
    private float attackCooldown;                // ���� ��Ÿ�� ������ ����

    public string monsterId { get; private set; } // ���� ���� ID (MonsterManager�κ��� ������ ��ȸ��)

    /// <summary>
    /// ���� ���� �� MonsterSpawner���� ID�� ���޹޾� �ʱ�ȭ
    /// </summary>
    public void Initialize(string monsterId)
    {
        this.monsterId = monsterId;
    }

    private void Start()
    {
        // �÷��̾� ã��
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // ���� ������ �ε�
        monsterData = MonsterManager.Instance.GetMonsterData(monsterId);
        if (monsterData == null)
        {
            Debug.LogError($"MonsterData�� ã�� �� �����ϴ�: {monsterId}");
            return;
        }

        // ������ ��� ���� �ʱ�ȭ
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

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            // �÷��̾� �������� �̵�
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);

            //// ���Ÿ� ������ ��쿡�� ��Ÿ�� üũ �� ����
            //attackCooldown -= Time.deltaTime;
            //if (monsterData.attackPattern == AttackPattern.Ranged &&
            //    distance <= attackRange &&
            //    attackCooldown <= 0)
            //{
            //    Attack();
            //}
        }
        else
        {
            // �÷��̾ Ž�� ���� �ۿ� ���� ��� ����
            Move(Vector2.zero);
        }
    }

    /// <summary>
    /// ���Ÿ� ���͸� ȣ��Ǵ� ���� �Լ�
    /// - �߻�ü�� �����ϰ� �ʱ�ȭ��
    /// - ���� ���ʹ� �� �Լ� ������� ����
    /// </summary>
    public override void Attack()
    {
        //isAttacking = true;
        //attackCooldown = 1f / attackSpeed;

        //if (monsterData.attackPattern == AttackPattern.Ranged)
        //{
        //    if (projectilePrefab != null && firePoint != null)
        //    {
        //        // �߻�ü ����
        //        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        //        Vector2 dir = (player.position - firePoint.position).normalized;
        //        proj.GetComponent<Projectile>().Initialize(dir, attackDamage);
        //    }
        //}
    }

    /// <summary>
    /// �÷��̾�� �浹 �� ������ �ο�
    /// - ���� ������ ���� ����
    /// - �ִϸ��̼� ���� ��� �������� ��
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive) return;

        if (other.CompareTag("Player") && monsterData.attackPattern == AttackPattern.Melee)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                //player.TakeDamage(attackDamage); // �÷��̾� ��ũ��Ʈ���� ������ �ִ� �޼��� �����;���
                Debug.Log($"{characterName}�� �÷��̾�� �浹�Ͽ� {attackDamage} �������� ����");
            }
        }
    }
}
