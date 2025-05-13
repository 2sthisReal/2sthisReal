using UnityEngine;

/// <summary>
/// Monster Ŭ������ BaseCharacter�� ��ӹ޾� ������ �⺻ �ൿ�� �����մϴ�.
/// </summary>
public class Monster : BaseCharacter
{
    [Header("���� �� ���� ����")]
    public float detectionRange = 15f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    protected Transform player;              // protected�� ����
    protected MonsterData monsterData;       // protected�� ����
    protected float attackCooldown;

    public string monsterId { get; private set; }

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

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);

            attackCooldown -= Time.deltaTime;
            if (monsterData.attackPattern == AttackPattern.Ranged &&
                distance <= attackRange &&
                attackCooldown <= 0)
            {
                Attack();
            }
        }
        else
        {
            Move(Vector2.zero);
        }
    }

    public override void Attack()
    {
        // �⺻ ���ʹ� �������� ����. ���� Ÿ�Կ��� ����
    }
}