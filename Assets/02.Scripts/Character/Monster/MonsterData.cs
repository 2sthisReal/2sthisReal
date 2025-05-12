using System;
using System.Collections.Generic;

/// <summary>
/// MonsterData 클래스는 각 몬스터의 세부 데이터를 담는 클래스입니다.
/// 이 데이터는 JSON으로 직렬화되어 저장/로드되며,
/// 스폰 시 몬스터의 능력치와 행동을 결정합니다.
/// </summary>
[Serializable]
public class MonsterData
{
    public string id;                   // 몬스터 고유 ID (예: "goblin_archer")
    public string monsterName;          // 몬스터 이름 (예: "고블린 궁수")
    public int level;                   // 몬스터 레벨
    public float maxHealth;             // 최대 체력
    public float attackDamage;          // 공격력
    public float attackSpeed;           // 초당 공격 횟수
    public float moveSpeed;             // 이동 속도
    public float attackRange;           // 공격 범위 (사거리)
    public string prefabPath;           // 프리팹 경로 (Resources 폴더 기준, 예: "Prefabs/Monsters/GoblinArcher")
    public MonsterType type;            // 몬스터 유형 (일반, 정예, 보스 등)
    public string[] dropItems;          // 드롭 아이템 목록 (아이템 ID 배열)
    public float dropRate;              // 아이템 드롭 확률 (0~1)
    public int expReward;               // 처치 시 주는 경험치
    public string spritePath;           // 몬스터 경로 명시적 표시

    // 추가: 궁수의 전설 스타일 공격 패턴
    public AttackPattern attackPattern; // 공격 방식 (근접, 원거리 등)
}

/// <summary>
/// 몬스터의 종류를 나타내는 열거형입니다.
/// - Normal: 일반 몬스터
/// - Elite: 정예 몬스터
/// - Boss: 보스 몬스터
/// - Passive: 공격하지 않는 몬스터
/// </summary>
public enum MonsterType
{
    Normal,
    Elite,
    Boss,
    Passive
}

/// <summary>
/// 몬스터의 공격 패턴을 정의하는 열거형입니다.
/// - Melee: 근접 공격
/// - Ranged: 원거리 발사체 공격
/// - Charge: 돌진형 공격 (확장용)
/// </summary>
public enum AttackPattern
{
    Melee,
    Ranged,
    Charge
}

/// <summary>
/// 여러 몬스터 데이터를 담는 데이터베이스 컨테이너.
/// 이 객체는 JSON의 최상위 구조로 사용됩니다.
/// 예시:
/// {
///   "monsters": [ ... 몬스터 목록 ... ]
/// }
/// </summary>
[Serializable]
public class MonsterDatabase
{
    public List<MonsterData> monsters = new List<MonsterData>();
}
