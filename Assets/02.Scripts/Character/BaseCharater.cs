using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("기본 스탯")]  // Inspector 창에서 그룹화하여 표시
    public string characterName;   // 캐릭터의 이름
    public int level = 1;          // 캐릭터의 레벨, 기본값 1
    public float maxHealth;        // 최대 체력
    public float currentHealth;    // 현재 체력
    public float moveSpeed;        // 이동 속도
    public float attackDamage;     // 공격력
    public float attackSpeed;      // 공격 속도(초당 공격 횟수)
    public float attackRange;      // 공격 범위

    [Header("상태")]  // 캐릭터의 현재 상태 관련 변수
    public bool isAlive = true;    // 생존 상태, 기본값 true
    public bool isAttacking = false; // 공격 중인지 여부
    public bool isMoving = false;  // 이동 중인지 여부

    // 자식 클래스에서 접근 가능한 컴포넌트 참조
    protected Animator animator;    // 애니메이션 제어를 위한 Animator 컴포넌트
    protected Rigidbody2D rb;      // 물리 이동을 위한 Rigidbody2D 컴포넌트

    // 초기화 메서드, virtual로 선언하여 자식 클래스에서 확장 가능
    protected virtual void Awake()
    {
        // 필요한 컴포넌트 참조 가져오기
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // 시작 시 현재 체력을 최대 체력으로 설정
        currentHealth = maxHealth;
    }

    // 캐릭터 이동 처리 메서드
    public virtual void Move(Vector2 direction)
    {
        // 사망 상태면 이동하지 않음
        if (!isAlive) return;

        // 정규화된 방향 벡터에 이동 속도와 시간을 곱하여 이동량 계산
        Vector2 movement = direction.normalized * moveSpeed * Time.deltaTime;
        // Rigidbody2D를 사용하여 물리적으로 위치 이동
        rb.MovePosition(rb.position + movement);

        // 방향 벡터의 크기가 0보다 크면 이동 중인 상태로 설정
        isMoving = direction.magnitude > 0;

        if (isMoving)
        {
            // 이동 중이면 애니메이션 파라미터 설정
            animator.SetBool("IsMoving", true);

            // 캐릭터 좌우 방향 설정 (x가 양수면 오른쪽, 음수면 왼쪽)
            if (direction.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
            }
        }
        else
        {
            // 이동하지 않으면 애니메이션 파라미터 해제
            animator.SetBool("IsMoving", false);
        }
    }

    // 공격 메서드 - 추상 메서드로 선언하여 자식 클래스에서 반드시 구현하도록 함
    public abstract void Attack();

    // 데미지 처리 메서드
    public virtual void TakeDamage(float damage)
    {
        // 사망 상태면 데미지를 받지 않음
        if (!isAlive) return;

        // 현재 체력에서 데미지만큼 감소
        currentHealth -= damage;

        // 데미지 이펙트 또는 UI 표시 등을 여기서 처리

        // 체력이 0 이하면 사망 처리
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // 피격 애니메이션 트리거
            animator.SetTrigger("Hit");
        }
    }

    // 사망 처리 메서드
    protected virtual void Die()
    {
        // 사망 상태로 변경
        isAlive = false;
        isAttacking = false;
        isMoving = false;

        // 사망 애니메이션 트리거
        animator.SetTrigger("Die");

        // 물리적 충돌 비활성화
        GetComponent<Collider2D>().enabled = false;
        // 물리 시뮬레이션 비활성화
        rb.simulated = false;
    }
}
