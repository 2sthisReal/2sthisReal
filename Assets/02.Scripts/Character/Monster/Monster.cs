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

    public void Initialize(string monsterId)
    {
        this.monsterId = monsterId;
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
        maxHealth = monsterData.maxHealth;
        currentHealth = maxHealth;
        moveSpeed = monsterData.moveSpeed;
        attackDamage = monsterData.attackDamage;
        attackSpeed = monsterData.attackSpeed;
        attackRange = monsterData.attackRange;
    }

    protected virtual void Update()
    {
        if (!isAlive || player == null) return;
        // ���� ���� �ȿ� ������ �� �⺻���� �ൿ ó�� (�ڽ� Ŭ�������� �ʿ� ���� �� �������̵�)
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);  // �̵�
            if (attackCooldown <= 0)
            {
                Attack();
                attackCooldown = 1f / attackSpeed;  // ���� ��ٿ�
            }
        }
        else
        {
            Move(Vector2.zero);  // �÷��̾ ���� �ۿ� ������ �̵� �� ��
        }
    }

    public abstract override void Attack();  // �ڽ� Ŭ�������� ���� ����� ����
}