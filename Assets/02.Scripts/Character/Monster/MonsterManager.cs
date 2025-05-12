using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// MonsterManager 클래스는 MonsterData.json 파일을 로드하고,
/// 각 몬스터 데이터를 Dictionary에 캐싱하여 빠르게 조회할 수 있게 합니다.
/// 싱글톤 패턴으로 구현되어 어디서든 Instance로 접근 가능합니다.
/// </summary>
public class MonsterManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static MonsterManager Instance { get; private set; }

    private MonsterDatabase monsterDatabase;                      // 전체 몬스터 데이터베이스
    private Dictionary<string, MonsterData> monsterDictionary = new(); // ID로 몬스터를 빠르게 찾기 위한 딕셔너리

    private void Awake()
    {
        // 싱글톤 패턴 처리: 이미 인스턴스가 있으면 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 유지

        LoadMonsterDatabase();          // 몬스터 데이터 로드
    }

    /// <summary>
    /// MonsterData.json 파일을 읽어서 몬스터 데이터를 메모리에 적재합니다.
    /// 파일이 없으면 경고 로그를 출력합니다.
    /// </summary>
    private void LoadMonsterDatabase()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "MonsterData.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            // JSON 데이터를 MonsterDatabase 객체로 변환
            monsterDatabase = JsonUtility.FromJson<MonsterDatabase>(json);

            // Dictionary에 캐싱
            foreach (var monster in monsterDatabase.monsters)
            {
                
                monsterDictionary[monster.id] = monster;
                Debug.Log($"[MonsterManager] 로드된 몬스터 ID: {monster.id}");
            }

            Debug.Log($"몬스터 데이터 로드 완료: {monsterDatabase.monsters.Count}개");
        }
        else
        {
            Debug.LogError($"MonsterData.json 파일을 찾을 수 없습니다. 경로: {filePath}");
            monsterDatabase = new MonsterDatabase();  // 비어있는 데이터베이스 초기화
        }
    }

    /// <summary>
    /// 몬스터 ID로 몬스터 데이터를 가져옵니다.
    /// 찾지 못하면 null을 반환합니다.
    /// </summary>
    /// <param name="monsterId">찾을 몬스터의 고유 ID</param>
    /// <returns>MonsterData 객체 또는 null</returns>
    public MonsterData GetMonsterData(string monsterId)
    {
        monsterDictionary.TryGetValue(monsterId, out var data);
        return data;
    }
}
