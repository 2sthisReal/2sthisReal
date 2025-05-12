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
    private float speed = 10f;     // �߻�ü �̵� �ӵ�
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
        Destroy(gameObject, 5f);  // 5�� �� �ڵ� ���� (�� �°� �������)
    }

    private void Update()
    {
        // �� �����Ӹ��� ������ �������� �̵�
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // �÷��̾� �±׸� ���� ������Ʈ�� ������ ������ ����
        if (col.CompareTag("Player"))
        {
            // PlayerHealth ������Ʈ�� ������ ������ ����
            col.GetComponent<PlayerHealth>()?.TakeDamage(damage);

            // �浹 �� �߻�ü ����
            Destroy(gameObject);
        }
    }
}
