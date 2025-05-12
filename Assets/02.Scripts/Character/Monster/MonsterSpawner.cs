using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonsterSpawner�� ���͸� �ֱ������� ��ȯ�ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private float spawnRadius = 10f;               // ��ȯ �ݰ�
    [SerializeField] private int maxMonsters = 5;                   // ���ÿ� ������ �� �ִ� �ִ� ���� ��
    [SerializeField] private float spawnInterval = 5f;              // ���� ��ȯ ����
    [SerializeField] private Transform spawnPointParent;            // ��ȯ ��ġ�� �ڽ����� ������ �θ� ������Ʈ

    [Header("���� ����")]
    [SerializeField] private List<string> monsterIdsToSpawn = new(); // ��ȯ�� ���� ID ���

    private List<Transform> spawnPoints = new();                    // ���� ���� ��ȯ ��ġ ���
    private List<Monster> activeMonsters = new();                   // ���� ����ִ� ���� ����Ʈ
    private float nextSpawnTime;                                    // ���� ��ȯ ����

    private void Start()
    {
        InitializeSpawnPoints();                                     // ��ȯ ��ġ �ʱ�ȭ
        nextSpawnTime = Time.time + spawnInterval;                  // ù ��ȯ Ÿ�̹� ����
    }

    private void Update()
    {
        // ���� ���ʹ� ����Ʈ���� ����
        activeMonsters.RemoveAll(monster => monster == null || !monster.isAlive);

        // ���� ���� �ѵ����� ����, ��ȯ Ÿ�̹��� �Ǹ� ��ȯ ����
        if (activeMonsters.Count < maxMonsters && Time.time >= nextSpawnTime)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    /// <summary>
    /// ��ȯ ��ġ ����� �ʱ�ȭ
    /// - spawnPointParent�� ������ �� �ڽĵ��� ���
    /// - ������ ���� ��ġ�� �⺻ ��ȯ ��ġ�� ���
    /// </summary>
    private void InitializeSpawnPoints()
    {
        spawnPoints.Clear();

        if (spawnPointParent != null)
        {
            foreach (Transform child in spawnPointParent)
                spawnPoints.Add(child);
        }

        if (spawnPoints.Count == 0)
            spawnPoints.Add(transform);
    }

    /// <summary>
    /// ���� �� ������ ��ȯ
    /// </summary>
    private void SpawnMonster()
    {
        if (monsterIdsToSpawn.Count == 0)
        {
            Debug.LogWarning("������ ���� ID�� �����ϴ�.");
            return;
        }

        // �������� ���� ID ���� �� ������ �ε�
        string monsterId = monsterIdsToSpawn[Random.Range(0, monsterIdsToSpawn.Count)];
        MonsterData monsterData = MonsterManager.Instance.GetMonsterData(monsterId);

        if (monsterData == null)
        {
            Debug.LogError($"MonsterData �� ã��: {monsterId}");
            return;
        }

        // ��ȯ ��ġ ���
        Vector3 spawnPos = GetRandomSpawnPosition();
        Debug.Log($"���� ���� �õ�: ID={monsterId}, ���={monsterData.prefabPath}, ��ġ={spawnPos}");

        // Resources �������� ������ �ε�
        GameObject prefab = Resources.Load<GameObject>(monsterData.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"������ �ε� ����: {monsterData.prefabPath}");
            return;
        }

        // ���� �ν��Ͻ� ����
        GameObject monsterObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (monsterObj != null)
        {
            // �ڽ� ������Ʈ �� MainSprite�� �����ϴ��� Ȯ��
            Transform mainSpriteTr = monsterObj.transform.Find("MainSprite");
            Debug.Log(mainSpriteTr != null ? "MainSprite ã��" : "MainSprite �� ã��");

            // Monster ��ũ��Ʈ Ȯ�� �� �ʱ�ȭ
            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                CheckAndFixSprites(monsterObj, monsterData);
                monster.Initialize(monsterId);               // ���� ID ����
                activeMonsters.Add(monster);                // Ȱ��ȭ�� ���ͷ� ���
            }
            else
            {
                Debug.LogError("Monster ������Ʈ�� ã�� ���߽��ϴ�.");
            }
        }
    }

    /// <summary>
    /// ���� ������Ʈ�� ��������Ʈ/�ִϸ����� Ȯ�� �α�
    /// ������ - ���ҽ� ���� ���� üũ
    /// </summary>
    private void CheckAndFixSprites(GameObject monsterObj, MonsterData monsterData)
    {
        Transform mainSpriteTr = monsterObj.transform.Find("MainSprite");
        if (mainSpriteTr != null)
        {
            Animator animator = mainSpriteTr.GetComponent<Animator>();
            Debug.Log(animator != null
                ? "Animator �����. SpriteRenderer�� Animator�� ������"
                : "Animator ����. SpriteRenderer ���� ���� �ʿ��� �� ����");

            SpriteRenderer mainRenderer = mainSpriteTr.GetComponent<SpriteRenderer>();
            Debug.Log(mainRenderer != null
                ? $"MainSprite�� SpriteRenderer ����. ���� sprite: {(mainRenderer.sprite != null ? mainRenderer.sprite.name : "null")}"
                : "MainSprite�� SpriteRenderer ����");
        }
    }

    /// <summary>
    /// ���� ��ġ ��ȯ (��ȯ �ݰ� ������)
    /// </summary>
    private Vector3 GetRandomSpawnPosition()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector2 offset = Random.insideUnitCircle * spawnRadius;
        return spawnPoint.position + new Vector3(offset.x, offset.y, 0);
    }

    /// <summary>
    /// �����Ϳ��� ��ȯ �ݰ��� �ð������� �����ִ� �����
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if (spawnPointParent != null)
        {
            foreach (Transform child in spawnPointParent)
                Gizmos.DrawWireSphere(child.position, spawnRadius);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
