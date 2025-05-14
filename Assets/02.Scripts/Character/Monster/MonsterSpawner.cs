using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonsterSpawner는 지정된 위치에 몬스터를 소환하는 컴포넌트입니다.
/// 최대 몬스터 수 제한과 주기적 자동 소환을 지원합니다.
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    [Header("몬스터 설정")]
    [SerializeField] private List<string> normalMonsterIds = new();
    [SerializeField] private List<string> bossMonsterIds = new();
    [SerializeField] private bool isBossStage = false;

    public int CurrentStage { get; set; }
    private List<Monster> activeMonsters = new();
    private List<Vector3> spawnPositions = new();

    /// <summary>
    /// 외부 호출용: 지정된 위치에 몬스터를 스폰하고 스폰 위치 리스트에 추가
    /// </summary>
    public void SpawnMonsterInStage(Vector3 spawnPos)
    {
        if (!spawnPositions.Contains(spawnPos))
            spawnPositions.Add(spawnPos);

        if (isBossStage)
        {
            // 보스 스테이지에서는 보스 몬스터만 소환
            SpawnBossMonster(spawnPos);
        }
        else
        {
            // 일반 스테이지에서는 일반 몬스터 소환
            SpawnNormalMonster(spawnPos);
        }
    }

    /// <summary>
    /// 일반 몬스터 소환
    /// </summary>
    private void SpawnNormalMonster(Vector3 spawnPos)
    {
        if (normalMonsterIds.Count == 0)
        {
            Debug.LogWarning("스폰할 일반 몬스터 ID가 없습니다.");
            return;
        }

        string monsterId = normalMonsterIds[Random.Range(0, normalMonsterIds.Count)];
        SpawnMonsterInternal(monsterId, spawnPos);
    }

    /// <summary>
    /// 보스 몬스터 소환
    /// </summary>
    private void SpawnBossMonster(Vector3 spawnPos)
    {
        if (bossMonsterIds.Count == 0)
        {
            Debug.LogWarning("스폰할 보스 몬스터 ID가 없습니다.");
            return;
        }

        string bossId = bossMonsterIds[Random.Range(0, bossMonsterIds.Count)];
        SpawnMonsterInternal(bossId, spawnPos);
    }

    /// <summary>
    /// 실제 몬스터 스폰 로직 (몬스터 ID와 위치 받음)
    /// </summary>
    private void SpawnMonsterInternal(string monsterId, Vector3 spawnPos)
    {
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
                monster.Initialize(monsterId, CurrentStage);
                activeMonsters.Add(monster);
            }
            else
            {
                Debug.LogError("Monster 컴포넌트를 찾지 못했습니다.");
            }
        }
    }

    /// <summary>
    /// 보스 스테이지 설정 (StageManager에서 호출)
    /// </summary>
    public void SetBossStage(bool isBoss)
    {
        isBossStage = isBoss;
        ClearActiveMonsters();
    }

    /// <summary>
    /// 스테이지 변경 시 소환된 몬스터 정리
    /// </summary>
    public void ClearActiveMonsters()
    {
        spawnPositions.Clear();

        foreach (var monster in activeMonsters)
        {
            if (monster != null)
            {
                Destroy(monster.gameObject);
            }
        }

        activeMonsters.Clear();
    }
}