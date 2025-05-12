using UnityEngine;

/// <summary>
/// Monster 클래스는 BaseCharacter를 상속받아 실제 몬스터의 행동을 구현합니다.
/// - 플레이어를 추적해서 접근
/// - 일정 거리 이내에 들어오면 공격 (근접/원거리 모두 지원)
/// - MonsterData를 기반으로 스탯/패턴을 설정
/// </summary>
public class Monster : BaseCharacter
{
    [Header("추적 및 공격 설정")]
    public float detectionRange = 15f;         // 플레이어를 탐지하는 범위
    public GameObject projectilePrefab;        // 발사체 프리팹 (원거리 공격용)
    public Transform firePoint;                // 발사체 발사 위치

    private Transform player;                  // 추적할 플레이어 오브젝트
    private MonsterData monsterData;           // 이 몬스터의 데이터
    private float attackCooldown;              // 공격 쿨타임

    public string monsterId { get; private set; }  // 이 몬스터의 고유 ID

    /// <summary>
    /// 스폰 후 MonsterSpawner에서 ID를 전달받아 초기화할 때 호출
    /// </summary>
    public void Initialize(string monsterId)
    {
        this.monsterId = monsterId;
    }

    private void Start()
    {
        // 플레이어 찾기 (태그로 탐색)
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 몬스터 데이터 불러오기
        monsterData = MonsterManager.Instance.GetMonsterData(monsterId);
        if (monsterData == null)
        {
            Debug.LogError($"MonsterData를 찾을 수 없습니다: {monsterId}");
            return;
        }

        // 데이터 기반으로 스탯 초기화
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

        // 플레이어까지 거리 계산
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            // 플레이어를 향해 이동
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);

            // 공격 쿨다운 체크
            attackCooldown -= Time.deltaTime;
            if (distance <= attackRange && attackCooldown <= 0)
            {
                Attack();
            }
        }
        else
        {
            // 탐지 범위 밖이면 멈춤
            Move(Vector2.zero);
        }
    }

    /// <summary>
    /// 공격 동작 (MonsterData의 attackPattern에 따라 다르게 동작)
    /// </summary>
    public override void Attack()
    {
        isAttacking = true;
        attackCooldown = 1f / attackSpeed;  // 쿨타임 갱신

        if (monsterData.attackPattern == AttackPattern.Ranged)
        {
            // 원거리 공격: 발사체 생성
            if (projectilePrefab != null && firePoint != null)
            {
                // 발사체 인스턴스화
                GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

                // 방향 계산 후 발사체 초기화
                Vector2 dir = (player.position - firePoint.position).normalized;
                proj.GetComponent<Projectile>().Initialize(dir, attackDamage);
            }
            animator.SetTrigger("Attack");
        }
        else if (monsterData.attackPattern == AttackPattern.Melee)
        {
            // 근접 공격: 애니메이션만 실행 (데미지는 충돌 시 처리)
            animator.SetTrigger("Attack");
            Debug.Log($"{characterName} 근접 공격 실행!");
        }
        // 나중에 Charge 같은 추가 패턴도 여기에 구현 가능
    }
}
