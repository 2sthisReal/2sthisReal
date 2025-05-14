using System.Collections;
using UnityEngine;

/// <summary>
/// BossMonster는 다양한 패턴으로 공격하는 보스 몬스터입니다.
/// - 순차적인 FSM 구조로 4가지 패턴을 실행합니다.
/// </summary>
public class BossMonster : MonoBehaviour
{
    public GameObject straightProjectile;
    public GameObject fanProjectile;
    public GameObject spiralProjectile;
    public GameObject laserBeamPrefab;

    public Transform firePoint;
    public float patternCooldown = 2f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(PatternRoutine());
    }

    private IEnumerator PatternRoutine()
    {
        while (true)
        {
            yield return StraightShot();
            yield return new WaitForSeconds(patternCooldown);

            yield return FanShot();
            yield return new WaitForSeconds(patternCooldown);

            yield return SpiralShot();
            yield return new WaitForSeconds(patternCooldown);

            yield return RotatingLaser();
            yield return new WaitForSeconds(patternCooldown);
        }
    }

    /// <summary>
    /// 직선 투사체 1개를 플레이어 방향으로 발사
    /// </summary>
    private IEnumerator StraightShot()
    {
        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(straightProjectile, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * 5f;
        yield return null;
    }

    /// <summary>
    /// 부채꼴로 투사체 여러 개를 발사
    /// </summary>
    private IEnumerator FanShot()
    {
        int count = 5;
        float angleStep = 15f;
        float startAngle = -angleStep * (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            GameObject bullet = Instantiate(fanProjectile, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = dir * 5f;
        }

        yield return null;
    }

    /// <summary>
    /// 일정 시간동안 회전하며 투사체를 나선형으로 퍼뜨림
    /// </summary>
    private IEnumerator SpiralShot()
    {
        int totalShots = 30;
        float delay = 0.05f;
        float angle = 0f;

        for (int i = 0; i < totalShots; i++)
        {
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            GameObject bullet = Instantiate(spiralProjectile, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = dir * 4f;

            angle += 12f; // 회전 각도 증가 (스파이럴 효과)
            yield return new WaitForSeconds(delay);
        }
    }

    /// <summary>
    /// 레이저를 생성하고 회전시키는 패턴 (애니메이션용)
    /// </summary>
    private IEnumerator RotatingLaser()
    {
        // 레이저를 생성하고 회전 시작
        GameObject laser = Instantiate(laserBeamPrefab, firePoint.position, Quaternion.identity);
        laser.transform.parent = transform; // 보스 기준 회전

        float duration = 3f;
        float rotationSpeed = 90f; // 초당 회전 각도

        float timer = 0f;
        while (timer < duration)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime); // 보스 자체 회전
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(laser);
    }
}
