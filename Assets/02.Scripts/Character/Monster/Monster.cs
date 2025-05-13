using UnityEngine;

/// <summary>
/// Monster 클래스는 BaseCharacter를 상속받아 몬스터의 기본 행동을 정의합니다.
/// - 플레이어를 탐지하고 이동
/// - 원거리 몬스터만 공격 메서드 호출
/// - 근접 몬스터는 충돌 시 데미지 입힘
/// </summary>
public class Monster : BaseCharacter
{
    [Header("추적 및 공격 설정")]
    public float detectionRange = 15f;           // 플레이어 탐지 범위

    private Transform player;                    // 추적 대상 (플레이어)
    private MonsterData monsterData;             // 몬스터 능력치 및 공격 타입
    private float attackCooldown;                // 공격 쿨타임 관리용 변수

    public string monsterId { get; private set; } // 고유 몬스터 ID (MonsterManager로부터 데이터 조회용)

    /// <summary>
    /// 몬스터 생성 시 MonsterSpawner에서 ID를 전달받아 초기화
    /// </summary>
    public void Initialize(string monsterId)
    {
        this.monsterId = monsterId;
    }

    private void Start()
    {
        // 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 몬스터 데이터 로드
        monsterData = MonsterManager.Instance.GetMonsterData(monsterId);
        if (monsterData == null)
        {
            Debug.LogError($"MonsterData를 찾을 수 없습니다: {monsterId}");
            return;
        }

        // 데이터 기반 스탯 초기화
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
            // 플레이어 방향으로 이동
            Vector2 dir = (player.position - transform.position).normalized;
            Move(dir);

            //// 원거리 공격인 경우에만 쿨타임 체크 및 공격
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
            // 플레이어가 탐지 범위 밖에 있을 경우 멈춤
            Move(Vector2.zero);
        }
    }

    /// <summary>
    /// 원거리 몬스터만 호출되는 공격 함수
    /// - 발사체를 생성하고 초기화함
    /// - 근접 몬스터는 이 함수 사용하지 않음
    /// </summary>
    public override void Attack()
    {
        //isAttacking = true;
        //attackCooldown = 1f / attackSpeed;

        //if (monsterData.attackPattern == AttackPattern.Ranged)
        //{
        //    if (projectilePrefab != null && firePoint != null)
        //    {
        //        // 발사체 생성
        //        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        //        Vector2 dir = (player.position - firePoint.position).normalized;
        //        proj.GetComponent<Projectile>().Initialize(dir, attackDamage);
        //    }
        //}
    }

    /// <summary>
    /// 플레이어와 충돌 시 데미지 부여
    /// - 근접 몬스터일 때만 동작
    /// - 애니메이션 없이 즉시 데미지를 줌
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive) return;

        if (other.CompareTag("Player") && monsterData.attackPattern == AttackPattern.Melee)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                //player.TakeDamage(attackDamage); // 플레이어 스크립트에서 데미지 주는 메서드 가져와야함
                Debug.Log($"{characterName}이 플레이어에게 충돌하여 {attackDamage} 데미지를 입힘");
            }
        }
    }
}
