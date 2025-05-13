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
        // 추적 범위 안에 들어왔을 때 기본적인 행동 처리 (자식 클래스에서 필요 없을 시 오버라이드)
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);  // 이동
            if (attackCooldown <= 0)
            {
                Attack();
                attackCooldown = 1f / attackSpeed;  // 공격 쿨다운
            }
        }
        else
        {
            Move(Vector2.zero);  // 플레이어가 범위 밖에 있으면 이동 안 함
        }
    }

    public abstract override void Attack();  // 자식 클래스에서 공격 방식을 정의
}