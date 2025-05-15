using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// MonsterManager Ŭ������ MonsterData.json ������ �ε��ϰ�,
/// �� ���� �����͸� Dictionary�� ĳ���Ͽ� ������ ��ȸ�� �� �ְ� �մϴ�.
/// �̱��� �������� �����Ǿ� ��𼭵� Instance�� ���� �����մϴ�.
/// </summary>
public class MonsterManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static MonsterManager Instance { get; private set; }

    private MonsterDatabase monsterDatabase;                      // ��ü ���� �����ͺ��̽�
    private Dictionary<string, MonsterData> monsterDictionary = new(); // ID�� ���͸� ������ ã�� ���� ��ųʸ�

    private void Awake()
    {
        // �̱��� ���� ó��: �̹� �ν��Ͻ��� ������ �ı�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // �� ��ȯ �ÿ��� ����

        LoadMonsterDatabase();          // ���� ������ �ε�
    }

    /// <summary>
    /// MonsterData.json ������ �о ���� �����͸� �޸𸮿� �����մϴ�.
    /// ������ ������ ��� �α׸� ����մϴ�.
    /// </summary>
    private void LoadMonsterDatabase()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "MonsterData.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            // JSON �����͸� MonsterDatabase ��ü�� ��ȯ
            monsterDatabase = JsonUtility.FromJson<MonsterDatabase>(json);

            // Dictionary�� ĳ��
            foreach (var monster in monsterDatabase.monsters)
            {
                
                monsterDictionary[monster.id] = monster;
                Debug.Log($"[MonsterManager] �ε�� ���� ID: {monster.id}");
            }

            Debug.Log($"���� ������ �ε� �Ϸ�: {monsterDatabase.monsters.Count}��");
        }
        else
        {
            Debug.LogError($"MonsterData.json ������ ã�� �� �����ϴ�. ���: {filePath}");
            monsterDatabase = new MonsterDatabase();  // ����ִ� �����ͺ��̽� �ʱ�ȭ
        }
    }

    /// <summary>
    /// ���� ID�� ���� �����͸� �����ɴϴ�.
    /// ã�� ���ϸ� null�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="monsterId">ã�� ������ ���� ID</param>
    /// <returns>MonsterData ��ü �Ǵ� null</returns>
    public MonsterData GetMonsterData(string monsterId)
    {
        monsterDictionary.TryGetValue(monsterId, out var data);
        return data;
    }
}
