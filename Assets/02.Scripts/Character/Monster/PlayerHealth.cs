using UnityEngine;

/// <summary>
/// PlayerHealth Ŭ������ �÷��̾��� ü�� ���� �� �ǰ�/��� ó���� ����մϴ�.
/// - �������� ������ ü���� ����
/// - ü���� 0 ���Ϸ� �������� ��� ó�� (������ ����)
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;       // �÷��̾��� �ִ� ü��
    private float currentHealth;         // ���� ü��

    private void Start()
    {
        currentHealth = maxHealth;       // ���� �� ü�� Ǯ�� ä��
    }

    /// <summary>
    /// �ܺο��� �� �޼��带 ȣ���� �������� ���� �� �ֽ��ϴ�.
    /// (��: ������ �߻�ü�� �浹���� ��)
    /// </summary>
    /// <param name="dmg">���� ������ ��</param>
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;   // ü�� ����
        Debug.Log($"�÷��̾� �ǰ�: {dmg} / ���� ü��: {currentHealth}");

        // ��� ���� üũ
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// �÷��̾� ��� �� ȣ��Ǵ� �޼���.
    /// ���� ���ӿ����� ���� ���� ȭ�� ȣ��, ������ ó�� ���� �����ϸ� ��.
    /// </summary>
    private void Die()
    {
        Debug.Log("�÷��̾� ���!");
        // TODO: ��� �ִϸ��̼�, ���� ���� UI �� �߰� ���� �ʿ�
    }
}
