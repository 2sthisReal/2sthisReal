using System.Collections;
using UnityEngine;

/// <summary>
/// BossMonster�� �پ��� �������� �����ϴ� ���� �����Դϴ�.
/// - �������� FSM ������ 4���� ������ �����մϴ�.
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
    /// ���� ����ü 1���� �÷��̾� �������� �߻�
    /// </summary>
    private IEnumerator StraightShot()
    {
        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(straightProjectile, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * 5f;
        yield return null;
    }

    /// <summary>
    /// ��ä�÷� ����ü ���� ���� �߻�
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
    /// ���� �ð����� ȸ���ϸ� ����ü�� ���������� �۶߸�
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

            angle += 12f; // ȸ�� ���� ���� (�����̷� ȿ��)
            yield return new WaitForSeconds(delay);
        }
    }

    /// <summary>
    /// �������� �����ϰ� ȸ����Ű�� ���� (�ִϸ��̼ǿ�)
    /// </summary>
    private IEnumerator RotatingLaser()
    {
        // �������� �����ϰ� ȸ�� ����
        GameObject laser = Instantiate(laserBeamPrefab, firePoint.position, Quaternion.identity);
        laser.transform.parent = transform; // ���� ���� ȸ��

        float duration = 3f;
        float rotationSpeed = 90f; // �ʴ� ȸ�� ����

        float timer = 0f;
        while (timer < duration)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime); // ���� ��ü ȸ��
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(laser);
    }
}
