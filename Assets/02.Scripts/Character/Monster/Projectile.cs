using UnityEngine;

/// <summary>
/// Projectile 클래스는 몬스터(또는 플레이어)의 발사체를 제어합니다.
/// - 일정 속도로 앞으로 이동
/// - 일정 시간 후 자동 삭제
/// - 플레이어에 충돌 시 데미지를 입히고 사라짐
/// </summary>
public class Projectile : MonoBehaviour
{
    private Vector2 direction;     // 발사 방향
    private float speed = 3f;     // 발사체 이동 속도
    private float damage;          // 발사체가 입힐 데미지

    /// <summary>
    /// 발사체 초기화 메서드.
    /// 생성 직후 방향과 데미지를 설정해줘야 합니다.
    /// </summary>
    /// <param name="dir">발사 방향 (단위 벡터)</param>
    /// <param name="dmg">데미지</param>
    public void Initialize(Vector2 dir, float dmg)
    {
        direction = dir;
        damage = dmg;
    }

    private void Update()
    {
        // 방향대로 이동
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // 이동 방향을 기준으로 회전 (자연스럽게 날아가는 느낌)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // 플레이어 태그를 가진 오브젝트에 닿으면 데미지 적용
        if (col.CompareTag("Player"))
        {
            // PlayerHealth 컴포넌트가 있으면 데미지 적용
            col.GetComponent<PlayerHealth>()?.TakeDamage(damage);

            // 충돌 후 발사체 삭제
            Destroy(gameObject);
        }
        else if (col.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
