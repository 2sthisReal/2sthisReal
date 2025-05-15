using UnityEngine;

/// <summary>
/// Monster Ŭ������ BaseCharacter�� ��ӹ޾� ������ �⺻ �ൿ�� �����մϴ�.
/// </summary>
public abstract class Monster : BaseCharacter
{
    [Header("���� �� ���� ����")]
    public float detectionRange = 15f;  // ���� ����
    protected Transform player;        // �÷��̾� Ʈ������ (�ڽ� Ŭ�������� ���)
    protected MonsterData monsterData; // ���� ������
    public string monsterId { get; private set; }
    public float attackCooldown;  // ���� ��ٿ� �߰�
    private int expReward;
    private int currentStage;

    public void Initialize(string monsterId, int currentStage)
    {
        this.monsterId = monsterId;
        this.currentStage = currentStage;
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        monsterData = MonsterManager.Instance.GetMonsterData(monsterId);
        if (monsterData == null)
        {
            Debug.LogError($"MonsterData�� ã�� �� �����ϴ�: {monsterId}");
            return;
        }
        characterName = monsterData.monsterName;
        
        SetMonsterStatWithStage(currentStage);
    }

    void SetMonsterStatWithStage(int stage)
    {
        maxHealth = monsterData.maxHealth;
        currentHealth = maxHealth;
        moveSpeed = monsterData.moveSpeed;
        attackDamage = monsterData.attackDamage;
        attackSpeed = monsterData.attackSpeed;
        attackRange = monsterData.attackRange;
        expReward = monsterData.expReward;

        for (int i = 0; i < stage; i++)
        {
            maxHealth += 100;
            currentHealth = maxHealth;
            attackDamage += 10;
            expReward += 30;
        }
    }

    protected virtual void Update()
    {
        if (!isAlive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            Vector2 moveDir = GetMoveDirection();

            Move(moveDir);

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
    }

    public abstract override void Attack();  // �ڽ� Ŭ�������� ���� ����� ����
    protected override void Die()
    {
        base.Die(); // ü�� 0 ó�� �� �⺻ ��� ó��

        // �÷��̾�� ����ġ ����
        if (player != null && monsterData != null)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.GetExp(monsterData.expReward);
                Debug.Log($"{monsterData.monsterName} óġ �� EXP {monsterData.expReward} ȹ��");
            }
        }

        // ���� ����
        Destroy(gameObject);
    }
    protected virtual Vector2 GetMoveDirection()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float avoidDistance = 0.5f;
        LayerMask wallLayer = LayerMask.GetMask("Wall");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, avoidDistance, wallLayer);
        if (hit.collider != null)
        {
            // ���� ������ �� ���� �������� ȸ��
            return Vector2.Perpendicular(dirToPlayer).normalized;
        }

        return dirToPlayer;
    }



}