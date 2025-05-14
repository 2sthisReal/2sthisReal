using System.Collections;
using UnityEngine;

public class BossMonster : Monster
{
    // ���� �ʵ� ����
    public GameObject shockwaveParticlePrefab;
    public GameObject straightProjectile;
    public GameObject fanProjectile;
    public Transform firePoint;
    public float patternCooldown = 2f;

    private Coroutine patternCoroutine;

    protected override void Start()
    {
        base.Start(); // Monster�� Start() ȣ�� (������ �ʱ�ȭ ��)

        if (player != null)
            patternCoroutine = StartCoroutine(PatternRoutine());
    }

    protected override void Update()
    {

    }

    public override void Attack()
    {
        // ������ Attack()�� ���� ������� �ʰ� FSM �������� ������
        // �ʿ� �� ������ ���� ���� ���� ������ ���� �� ����
    }

    private IEnumerator PatternRoutine()
    {
        while (true)
        {
            yield return StraightShot();
            yield return new WaitForSeconds(patternCooldown);

            yield return FanShot();
            yield return new WaitForSeconds(patternCooldown);

            yield return DashAndShockwave();
            yield return new WaitForSeconds(patternCooldown);
        }
    }

    private IEnumerator StraightShot()
    {
        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(straightProjectile, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().Initialize(dir, 10f);
        yield return null;
    }

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
            bullet.GetComponent<Projectile>().Initialize(dir.normalized, 8f);
        }

        yield return null;
    }

    private IEnumerator DashAndShockwave()
    {
        float dashSpeed = 10f;
        float dashDuration = 0.3f;
        float shockwaveRadius = 2.5f;
        float shockwaveDamage = 20f;

        Vector2 dashDir = (player.position - transform.position).normalized;
        float timer = 0f;

        while (timer < dashDuration)
        {
            transform.Translate(dashDir * dashSpeed * Time.deltaTime, Space.World);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        CameraShaker.Instance?.Shake(0.3f, 0.2f);

        if (shockwaveParticlePrefab != null)
            Instantiate(shockwaveParticlePrefab, transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, shockwaveRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("����� �ǰ�!");
                hit.GetComponent<Player>()?.TakeDamage(shockwaveDamage);
            }
        }

        DebugDrawCircle(transform.position, shockwaveRadius, Color.red, 0.5f);
    }

    private void DebugDrawCircle(Vector3 center, float radius, Color color, float duration)
    {
        int segments = 32;
        float angleStep = 360f / segments;

        Vector3 prev = center + new Vector3(radius, 0f, 0f);
        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 next = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            Debug.DrawLine(prev, next, color, duration);
            prev = next;
        }
    }

    protected override void Die()
    {
        base.Die();

        GameManager.Instance.ChangeState(GameState.Victory);
    }
}
