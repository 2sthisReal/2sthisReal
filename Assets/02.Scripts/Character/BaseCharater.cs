using System.Collections;
using UnityEngine;

/// <summary>
/// BaseCharacter 클래스는 몬스터, 플레이어 등 공통 캐릭터의
/// 기본적인 동작(이동, 피격, 사망 등)을 정의하는 추상 클래스입니다.
/// 
/// 주로:
/// - 체력/공격력 등 기본 스탯
/// - 이동 처리 (물리 기반)
/// - 피격/사망 처리
/// 
/// 를 제공합니다. Monster.cs 같은 실제 캐릭터들은 이 클래스를 상속받아 구현합니다.
/// </summary>
public abstract class BaseCharacter : MonoBehaviour
{
    [Header("기본 스탯")]
    public string characterName;         // 캐릭터 이름 (ex: 고블린 궁수)
    public int level = 1;                // 캐릭터 레벨
    public float maxHealth;              // 최대 체력
    public float currentHealth;          // 현재 체력
    public float moveSpeed;              // 이동 속도
    public float attackDamage;           // 공격력
    public float attackSpeed;            // 초당 공격 횟수
    public float attackRange;            // 공격 거리

    [Header("상태")]
    public bool isAlive = true;          // 생존 여부
    public bool isAttacking = false;     // 현재 공격 중인지 여부
    public bool isMoving = false;        // 현재 이동 중인지 여부

    // 컴포넌트 캐시
    protected Animator animator;         // 애니메이션 제어용
    protected Rigidbody2D rb;            // 물리 이동용

    /// <summary>
    /// 컴포넌트를 가져오고 체력을 초기화합니다.
    /// 자식 클래스에서 확장할 수 있도록 virtual 처리.
    /// </summary>
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;       // 시작할 때 체력을 최대치로 설정
    }

    /// <summary>
    /// 방향 벡터를 받아 캐릭터를 이동시키는 메서드입니다.
    /// 방향의 크기가 0이면 이동 중지로 처리됩니다.
    /// </summary>
    /// <param name="direction">이동할 방향 (예: (1, 0) → 오른쪽)</param>
    public virtual void Move(Vector2 direction)
    {
        if (!isAlive) return;   // 죽은 경우 이동 금지

        // 방향 벡터 정규화 후 속도/시간 반영해서 이동량 계산
        Vector2 movement = direction.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);  // 실제 이동

        // 이동 여부 체크해서 애니메이션 파라미터 설정
        isMoving = direction.magnitude > 0;
        animator.SetBool("IsMoving", isMoving);

        // 방향에 따라 캐릭터 좌우 반전
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
    }

    /// <summary>
    /// 공격 로직. 추상 메서드이므로 반드시 자식 클래스에서 구현해야 합니다.
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// 데미지를 받을 때 호출되는 메서드.
    /// 현재 체력을 감소시키고 사망 여부를 체크합니다.
    /// </summary>
    /// <param name="damage">받을 데미지 양</param>
    public virtual void TakeDamage(float damage)
    {
        if (!isAlive) return;   // 이미 죽었으면 무시

        currentHealth -= damage;  // 체력 감소

        if (currentHealth <= 0)
        {
            Die();  // 체력 0 이하일 경우 사망 처리
        }
        else
        {
            animator.SetTrigger("Hit");  // 피격 애니메이션
        }
    }

    /// <summary>
    /// 사망 처리 메서드.
    /// - isAlive 플래그를 끄고
    /// - 사망 애니메이션 실행
    /// - 충돌 판정 및 물리 시뮬레이션 비활성화
    /// </summary>
    protected virtual void Die()
    {
        isAlive = false;
        animator.SetTrigger("Die");

        // 충돌 비활성화 (다른 오브젝트에 더 이상 반응하지 않음)
        GetComponent<Collider2D>().enabled = false;

        // Rigidbody 시뮬레이션 끔 (중력 등도 더 이상 안 받음)
        rb.simulated = false;
    }
}
