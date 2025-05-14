using UnityEngine;

/// <summary>
/// Projectile Ŭ������ ����(�Ǵ� �÷��̾�)�� �߻�ü�� �����մϴ�.
/// - ���� �ӵ��� ������ �̵�
/// - ���� �ð� �� �ڵ� ����
/// - �÷��̾ �浹 �� �������� ������ �����
/// </summary>
public class Projectile : MonoBehaviour
{
    private Vector2 direction;     // �߻� ����
    private float speed = 3f;     // �߻�ü �̵� �ӵ�
    private float damage;          // �߻�ü�� ���� ������

    /// <summary>
    /// �߻�ü �ʱ�ȭ �޼���.
    /// ���� ���� ����� �������� ��������� �մϴ�.
    /// </summary>
    /// <param name="dir">�߻� ���� (���� ����)</param>
    /// <param name="dmg">������</param>
    public void Initialize(Vector2 dir, float dmg)
    {
        direction = dir;
        damage = dmg;
    }

    private void Update()
    {
        // ������ �̵�
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // �̵� ������ �������� ȸ�� (�ڿ������� ���ư��� ����)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"[Projectile] �浹 �߻�! name: {col.name}, tag: {col.tag}, layer: {LayerMask.LayerToName(col.gameObject.layer)}");

        if (col.CompareTag("Player"))
        {
            Debug.Log("�÷��̾ ����!");
            col.GetComponent<Player>()?.KnockbackPlayer(direction, 2f);
            col.GetComponent<Player>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (col.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
