using System;
using System.Collections.Generic;

/// <summary>
/// MonsterData Ŭ������ �� ������ ���� �����͸� ��� Ŭ�����Դϴ�.
/// �� �����ʹ� JSON���� ����ȭ�Ǿ� ����/�ε�Ǹ�,
/// ���� �� ������ �ɷ�ġ�� �ൿ�� �����մϴ�.
/// </summary>
[Serializable]
public class MonsterData
{
    public string id;                   // ���� ���� ID (��: "goblin_archer")
    public string monsterName;          // ���� �̸� (��: "��� �ü�")
    public int level;                   // ���� ����
    public float maxHealth;             // �ִ� ü��
    public float attackDamage;          // ���ݷ�
    public float attackSpeed;           // �ʴ� ���� Ƚ��
    public float moveSpeed;             // �̵� �ӵ�
    public float attackRange;           // ���� ���� (��Ÿ�)
    public string prefabPath;           // ������ ��� (Resources ���� ����, ��: "Prefabs/Monsters/GoblinArcher")
    public MonsterType type;            // ���� ���� (�Ϲ�, ����, ���� ��)
    public string[] dropItems;          // ��� ������ ��� (������ ID �迭)
    public float dropRate;              // ������ ��� Ȯ�� (0~1)
    public int expReward;               // óġ �� �ִ� ����ġ
    public string spritePath;           // ���� ��� ����� ǥ��

    // �߰�: �ü��� ���� ��Ÿ�� ���� ����
    public AttackPattern attackPattern; // ���� ��� (����, ���Ÿ� ��)
}

/// <summary>
/// ������ ������ ��Ÿ���� �������Դϴ�.
/// - Normal: �Ϲ� ����
/// - Elite: ���� ����
/// - Boss: ���� ����
/// - Passive: �������� �ʴ� ����
/// </summary>
public enum MonsterType
{
    Normal,
    Elite,
    Boss,
    Passive
}

/// <summary>
/// ������ ���� ������ �����ϴ� �������Դϴ�.
/// - Melee: ���� ����
/// - Ranged: ���Ÿ� �߻�ü ����
/// - Charge: ������ ���� (Ȯ���)
/// </summary>
public enum AttackPattern
{
    Melee,
    Ranged,
    Charge
}

/// <summary>
/// ���� ���� �����͸� ��� �����ͺ��̽� �����̳�.
/// �� ��ü�� JSON�� �ֻ��� ������ ���˴ϴ�.
/// ����:
/// {
///   "monsters": [ ... ���� ��� ... ]
/// }
/// </summary>
[Serializable]
public class MonsterDatabase
{
    public List<MonsterData> monsters = new List<MonsterData>();
}
