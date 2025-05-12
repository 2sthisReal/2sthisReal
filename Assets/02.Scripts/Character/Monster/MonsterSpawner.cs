using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonsterSpawner는 몬스터를 주기적으로 소환하는 컴포넌트입니다.
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private int maxMonsters = 5;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private Transform spawnPointParent;

    [Header("몬스터 설정")]
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
        activeMonsters.RemoveAll(monster => monster == null || !monster.isAlive);

        if (activeMonsters.Count < maxMonsters && Time.time >= nextSpawnTime)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnInterval;
        }
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
            Debug.LogWarning("스폰할 몬스터 ID가 없습니다.");
            return;
        }

        string monsterId = monsterIdsToSpawn[Random.Range(0, monsterIdsToSpawn.Count)];
        MonsterData monsterData = MonsterManager.Instance.GetMonsterData(monsterId);

        if (monsterData == null)
        {
            Debug.LogError($"MonsterData 못 찾음: {monsterId}");
            return;
        }

        Vector3 spawnPos = GetRandomSpawnPosition();
        Debug.Log($"몬스터 스폰 시도: ID={monsterId}, 경로={monsterData.prefabPath}, 위치={spawnPos}");

        GameObject prefab = Resources.Load<GameObject>(monsterData.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"프리팹 로드 실패: {monsterData.prefabPath}");
            return;
        }

        GameObject monsterObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (monsterObj != null)
        {
            Transform mainSpriteTr = monsterObj.transform.Find("MainSprite");
            Debug.Log(mainSpriteTr != null ? "MainSprite 찾음" : "MainSprite 못 찾음");

            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                CheckAndFixSprites(monsterObj, monsterData);
                monster.Initialize(monsterId);
                activeMonsters.Add(monster);
            }
            else
            {
                Debug.LogError("Monster 컴포넌트를 찾지 못했습니다.");
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
                ? "Animator 연결됨. SpriteRenderer는 Animator가 제어함"
                : "Animator 없음. SpriteRenderer 수동 설정 필요할 수 있음");

            SpriteRenderer mainRenderer = mainSpriteTr.GetComponent<SpriteRenderer>();
            Debug.Log(mainRenderer != null
                ? $"MainSprite에 SpriteRenderer 있음. 현재 sprite: {(mainRenderer.sprite != null ? mainRenderer.sprite.name : "null")}"
                : "MainSprite에 SpriteRenderer 없음");
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
}