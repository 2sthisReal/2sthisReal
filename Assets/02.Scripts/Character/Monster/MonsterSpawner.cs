using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonsterSpawner는 지정된 위치에 몬스터를 소환하는 컴포넌트입니다.
/// 최대 몬스터 수 제한과 주기적 자동 소환을 지원합니다.
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    [Header("몬스터 설정")]
    [SerializeField] private List<string> monsterIdsToSpawn = new();
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxMonsters = 10;

    private List<Monster> activeMonsters = new();
    private List<Vector3> spawnPositions = new();
    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        /*
        activeMonsters.RemoveAll(monster => monster == null || !monster.isAlive);

        if (Time.time >= nextSpawnTime && activeMonsters.Count < maxMonsters && spawnPositions.Count > 0)
        {
            Vector3 randomPos = spawnPositions[Random.Range(0, spawnPositions.Count)];
            SpawnMonsterInternal(randomPos);
            nextSpawnTime = Time.time + spawnInterval;
        }
        */
    }

    /// <summary>
    /// 외부 호출용: 지정된 위치에 몬스터를 스폰하고 스폰 위치 리스트에 추가
    /// </summary>
    public void SpawnMonsterInStage(Vector3 spawnPos)
    {
        if (!spawnPositions.Contains(spawnPos))
            spawnPositions.Add(spawnPos);

        SpawnMonsterInternal(spawnPos);
    }

    /// <summary>
    /// 실제 몬스터 스폰 로직 (위치만 받음)
    /// </summary>
    private void SpawnMonsterInternal(Vector3 spawnPos)
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
            Debug.LogError($"MonsterData를 찾을 수 없습니다: {monsterId}");
            return;
        }

        GameObject prefab = Resources.Load<GameObject>(monsterData.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"프리팹 로드 실패: {monsterData.prefabPath}");
            return;
        }

        GameObject monsterObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (monsterObj != null)
        {
            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                monster.Initialize(monsterId);
                activeMonsters.Add(monster);
            }
            else
            {
                Debug.LogError("Monster 컴포넌트를 찾지 못했습니다.");
            }
        }
    }
}
