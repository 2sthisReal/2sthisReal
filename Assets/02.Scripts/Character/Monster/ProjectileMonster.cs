using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMonster : Monster
{
    [Header("발사체 설정")]
    public GameObject projectilePrefab;  // 발사할 프로젝타일 프리팹
    public Transform firePoint;          // 발사 위치
    private float attackCooldownTimer;

    // 추가: 발사체 속도 변수
    public float projectileSpeed = 5f;
    // 추가: 스프라이트 렌더러 참조
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        // 스프라이트 렌더러 컴포넌트 가져오기
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Update()
    {
        if (!isAlive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 플레이어 방향 벡터 계산
        Vector2 directionVector = (player.position - transform.position).normalized;

        // 스프라이트 방향 설정 (플레이어 위치에 따라 좌우 반전)
        if (spriteRenderer != null)
        {
            if (player.position.x - transform.position.x > 0)
                spriteRenderer.flipX = false;
            else if (player.position.x - transform.position.x < 0)
                spriteRenderer.flipX = true;
        }

        // 플레이어 추적 (공격 범위보다 멀리 있을 때만)
        if (distance <= detectionRange && distance > attackRange)
        {
            Move(directionVector);
        }
        else if (distance <= attackRange)
        {
            // 공격 범위 내에 들어오면 이동 멈춤
            Move(Vector2.zero);
        }
        else
        {
            Move(Vector2.zero);
        }

        // 공격 쿨다운 처리
        attackCooldownTimer -= Time.deltaTime;
        if (distance <= attackRange && attackCooldownTimer <= 0f)
        {
            Attack();
            attackCooldownTimer = 1f / attackSpeed;
        }
    }

    public override void Attack()
    {
        if (projectilePrefab == null || firePoint == null || player == null)
        {
            Debug.LogWarning($"{gameObject.name}의 projectilePrefab, firePoint 또는 player가 설정되지 않았습니다.");
            return;
        }

        // 발사 방향 계산 (플레이어 방향)
        Vector2 directionVector = (player.position - firePoint.position).normalized;

        // 프로젝타일 생성 및 초기화
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // 프로젝타일에 방향 및 데미지만 전달
        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(directionVector, attackDamage);
        }

        // 공격 애니메이션 트리거
        animator.SetTrigger("Attack");
    }

    // 추가: 원거리 공격에 대한 Gizmo 시각화
    void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}