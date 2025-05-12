using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonsterSpawner는 몬스터를 주기적으로 소환하는 컴포넌트입니다.
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    [SerializeField] private float spawnRadius = 10f;               // 소환 반경
    [SerializeField] private int maxMonsters = 5;                   // 동시에 존재할 수 있는 최대 몬스터 수
    [SerializeField] private float spawnInterval = 5f;              // 몬스터 소환 간격
    [SerializeField] private Transform spawnPointParent;            // 소환 위치를 자식으로 가지는 부모 오브젝트

    [Header("몬스터 설정")]
    [SerializeField] private List<string> monsterIdsToSpawn = new(); // 소환할 몬스터 ID 목록

    private List<Transform> spawnPoints = new();                    // 실제 사용될 소환 위치 목록
    private List<Monster> activeMonsters = new();                   // 현재 살아있는 몬스터 리스트
    private float nextSpawnTime;                                    // 다음 소환 시점

    private void Start()
    {
        InitializeSpawnPoints();                                     // 소환 위치 초기화
        nextSpawnTime = Time.time + spawnInterval;                  // 첫 소환 타이밍 설정
    }

    private void Update()
    {
        // 죽은 몬스터는 리스트에서 제거
        activeMonsters.RemoveAll(monster => monster == null || !monster.isAlive);

        // 몬스터 수가 한도보다 적고, 소환 타이밍이 되면 소환 실행
        if (activeMonsters.Count < maxMonsters && Time.time >= nextSpawnTime)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    /// <summary>
    /// 소환 위치 목록을 초기화
    /// - spawnPointParent가 있으면 그 자식들을 사용
    /// - 없으면 현재 위치를 기본 소환 위치로 사용
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
    /// 몬스터 한 마리를 소환
    /// </summary>
    private void SpawnMonster()
    {
        if (monsterIdsToSpawn.Count == 0)
        {
            Debug.LogWarning("스폰할 몬스터 ID가 없습니다.");
            return;
        }

        // 랜덤으로 몬스터 ID 선택 후 데이터 로드
        string monsterId = monsterIdsToSpawn[Random.Range(0, monsterIdsToSpawn.Count)];
        MonsterData monsterData = MonsterManager.Instance.GetMonsterData(monsterId);

        if (monsterData == null)
        {
            Debug.LogError($"MonsterData 못 찾음: {monsterId}");
            return;
        }

        // 소환 위치 계산
        Vector3 spawnPos = GetRandomSpawnPosition();
        Debug.Log($"몬스터 스폰 시도: ID={monsterId}, 경로={monsterData.prefabPath}, 위치={spawnPos}");

        // Resources 폴더에서 프리팹 로드
        GameObject prefab = Resources.Load<GameObject>(monsterData.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"프리팹 로드 실패: {monsterData.prefabPath}");
            return;
        }

        // 몬스터 인스턴스 생성
        GameObject monsterObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (monsterObj != null)
        {
            // 자식 오브젝트 중 MainSprite이 존재하는지 확인
            Transform mainSpriteTr = monsterObj.transform.Find("MainSprite");
            Debug.Log(mainSpriteTr != null ? "MainSprite 찾음" : "MainSprite 못 찾음");

            // Monster 스크립트 확인 후 초기화
            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                CheckAndFixSprites(monsterObj, monsterData);
                monster.Initialize(monsterId);               // 몬스터 ID 전달
                activeMonsters.Add(monster);                // 활성화된 몬스터로 등록
            }
            else
            {
                Debug.LogError("Monster 컴포넌트를 찾지 못했습니다.");
            }
        }
    }

    /// <summary>
    /// 몬스터 오브젝트의 스프라이트/애니메이터 확인 로그
    /// 디버깅용 - 리소스 누락 여부 체크
    /// </summary>
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

    /// <summary>
    /// 랜덤 위치 반환 (소환 반경 내에서)
    /// </summary>
    private Vector3 GetRandomSpawnPosition()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector2 offset = Random.insideUnitCircle * spawnRadius;
        return spawnPoint.position + new Vector3(offset.x, offset.y, 0);
    }

    /// <summary>
    /// 에디터에서 소환 반경을 시각적으로 보여주는 기즈모
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
