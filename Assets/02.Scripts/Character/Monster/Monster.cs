using UnityEngine;

/// <summary>
/// Monster 클래스는 BaseCharacter를 상속받아 몬스터의 기본 행동을 정의합니다.
/// </summary>
public class Monster : BaseCharacter
{
    [Header("추적 및 공격 설정")]
    public float detectionRange = 15f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    protected Transform player;              // protected로 변경
    protected MonsterData monsterData;       // protected로 변경
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
            Debug.LogError($"MonsterData를 찾을 수 없습니다: {monsterId}");
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
        // 기본 몬스터는 공격하지 않음. 세부 타입에서 구현
    }
}