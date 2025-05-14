using UnityEngine;

/// <summary>
/// Monster 클래스는 BaseCharacter를 상속받아 몬스터의 기본 행동을 정의합니다.
/// </summary>
public abstract class Monster : BaseCharacter
{
    [Header("추적 및 공격 설정")]
    public float detectionRange = 15f;  // 추적 범위
    protected Transform player;        // 플레이어 트랜스폼 (자식 클래스에서 사용)
    protected MonsterData monsterData; // 몬스터 데이터
    public string monsterId { get; private set; }
    public float attackCooldown;  // 공격 쿨다운 추가

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

    public abstract override void Attack();  // 자식 클래스에서 공격 방식을 정의
    protected override void Die()
    {
        base.Die(); // 체력 0 처리 등 기본 사망 처리

        // 플레이어에게 경험치 지급
        if (player != null && monsterData != null)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.GetExp(monsterData.expReward);
                Debug.Log($"{monsterData.monsterName} 처치 → EXP {monsterData.expReward} 획득");
            }
        }

        // 몬스터 제거
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
            // 벽에 막혔을 때 수직 방향으로 회피
            return Vector2.Perpendicular(dirToPlayer).normalized;
        }

        return dirToPlayer;
    }



}