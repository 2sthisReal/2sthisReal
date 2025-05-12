using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonsterSpawner�� ���͸� �ֱ������� ��ȯ�ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private int maxMonsters = 5;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private Transform spawnPointParent;

    [Header("���� ����")]
    [SerializeField] private List<string> monsterIdsToSpawn = new();

    private List<Transform> spawnPoints = new();
    private List<Monster> activeMonsters = new();
    private float nextSpawnTime;

    private void Start()
    {
        InitializeSpawnPoints();
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        /*
        activeMonsters.RemoveAll(monster => monster == null || !monster.isAlive);

        if (activeMonsters.Count < maxMonsters && Time.time >= nextSpawnTime)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnInterval;
        }
        */
    }

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

    private void SpawnMonster()
    {
        if (monsterIdsToSpawn.Count == 0)
        {
            Debug.LogWarning("������ ���� ID�� �����ϴ�.");
            return;
        }

        string monsterId = monsterIdsToSpawn[Random.Range(0, monsterIdsToSpawn.Count)];
        MonsterData monsterData = MonsterManager.Instance.GetMonsterData(monsterId);

        if (monsterData == null)
        {
            Debug.LogError($"MonsterData �� ã��: {monsterId}");
            return;
        }

        Vector3 spawnPos = GetRandomSpawnPosition();
        Debug.Log($"���� ���� �õ�: ID={monsterId}, ���={monsterData.prefabPath}, ��ġ={spawnPos}");

        GameObject prefab = Resources.Load<GameObject>(monsterData.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"������ �ε� ����: {monsterData.prefabPath}");
            return;
        }

        GameObject monsterObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (monsterObj != null)
        {
            Transform mainSpriteTr = monsterObj.transform.Find("MainSprite");
            Debug.Log(mainSpriteTr != null ? "MainSprite ã��" : "MainSprite �� ã��");

            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                CheckAndFixSprites(monsterObj, monsterData);
                monster.Initialize(monsterId);
                activeMonsters.Add(monster);
            }
            else
            {
                Debug.LogError("Monster ������Ʈ�� ã�� ���߽��ϴ�.");
            }
        }
    }

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

    private Vector3 GetRandomSpawnPosition()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector2 offset = Random.insideUnitCircle * spawnRadius;
        return spawnPoint.position + new Vector3(offset.x, offset.y, 0);
    }

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

    public void SpawnMonsterInStage(Vector3 spawnPos)
    {
        if (monsterIdsToSpawn.Count == 0)
        {
            Debug.LogWarning("������ ���� ID�� �����ϴ�.");
            return;
        }

        string monsterId = monsterIdsToSpawn[Random.Range(0, monsterIdsToSpawn.Count)];
        MonsterData monsterData = MonsterManager.Instance.GetMonsterData(monsterId);

        if (monsterData == null)
        {
            Debug.LogError($"MonsterData �� ã��: {monsterId}");
            return;
        }

        GameObject prefab = Resources.Load<GameObject>(monsterData.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"������ �ε� ����: {monsterData.prefabPath}");
            return;
        }

        GameObject monsterObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (monsterObj != null)
        {
            Transform mainSpriteTr = monsterObj.transform.Find("MainSprite");
            Debug.Log(mainSpriteTr != null ? "MainSprite ã��" : "MainSprite �� ã��");

            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                CheckAndFixSprites(monsterObj, monsterData);
                monster.Initialize(monsterId);
                activeMonsters.Add(monster);
            }
            else
            {
                Debug.LogError("Monster ������Ʈ�� ã�� ���߽��ϴ�.");
            }
        }
    }
}