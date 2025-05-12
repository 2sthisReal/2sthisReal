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
    private float speed = 10f;     // 발사체 이동 속도
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
        Destroy(gameObject, 5f);  // 5초 후 자동 삭제 (안 맞고도 사라지게)
    }

    private void Update()
    {
        // 매 프레임마다 지정된 방향으로 이동
        transform.Translate(direction * speed * Time.deltaTime);
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
    }
}
