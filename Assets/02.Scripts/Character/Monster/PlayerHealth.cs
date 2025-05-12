using UnityEngine;

/// <summary>
/// PlayerHealth 클래스는 플레이어의 체력 관리 및 피격/사망 처리를 담당합니다.
/// - 데미지를 받으면 체력이 감소
/// - 체력이 0 이하로 떨어지면 사망 처리 (간단한 예제)
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;       // 플레이어의 최대 체력
    private float currentHealth;         // 현재 체력

    private void Start()
    {
        currentHealth = maxHealth;       // 시작 시 체력 풀로 채움
    }

    /// <summary>
    /// 외부에서 이 메서드를 호출해 데미지를 입힐 수 있습니다.
    /// (예: 몬스터의 발사체가 충돌했을 때)
    /// </summary>
    /// <param name="dmg">입힐 데미지 양</param>
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;   // 체력 감소
        Debug.Log($"플레이어 피격: {dmg} / 남은 체력: {currentHealth}");

        // 사망 여부 체크
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 플레이어 사망 시 호출되는 메서드.
    /// 실제 게임에서는 게임 오버 화면 호출, 리스폰 처리 등을 구현하면 됨.
    /// </summary>
    private void Die()
    {
        Debug.Log("플레이어 사망!");
        // TODO: 사망 애니메이션, 게임 오버 UI 등 추가 구현 필요
    }
}
