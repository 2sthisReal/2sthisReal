# GameManager 

이 문서는 GameManager가 관리하는 게임 상태 및 플레이어 관련 데이터(스킬, 장비, 펫 등)에 대한
활용법에 대한 설명을 담고 있습니다.

---

## 담당 범위 요약 (완료)

- 게임 상태 전환 (로비 > 게임 > 클리어 등)
- 스킬 / 장비 / 펫 선택 정보 저장
- 전투 진행 중 몬스터 수 카운트 및 스테이지 클리어 판정
- 게임 루프 종료 처리 (GameOver, Victory 등)

---

## GameManager 구조 요약

| 구성 요소 | 설명 |
|----------|-------|
| 'SelectedSkills' | 선택된 스킬 목록 |
| 'EquippedSlots' | 슬롯 기반 장비 (무기, 방어구, 악세서리2칸, 펫2칸) |
| 'GameState' | 현재 게임 상태 (MainMenu, Preparing, InGame, StageClear 등) |
| 'remainingEnemies' | 남은 몬스터 수 (0이면 클리어 처리) |

---

## 다른 시스템에서 사용할 수 있는 메서드

### 장비 관련
```
var weapon = GameManager.Instance.GetEquipment(EquipmentSlot.Weapon);
if (weapon != null)
  player.ApplyWeapon(weapon);

var pet1 = GameManager.Instance.GetEquipment(EquipmentSlot.Pet1);
```

### 스킬 관련
```
foreach(var skill in GameManager.Instance.GetSelectedSkills())
{
  Debug.Log(skill.skillName);
}

if ( GameManger.Instance.HasSkill("wingman"))
{
  petController.EnableProjectileBlocking();
}
```

### 상태 초기화(전투 종료 시)
```
GameManager.Instance.ResetPlayerSession();
```

### 장비 장착 예시 (UI/Inventory 시스템과 연동 필요)
```
if (GameManager.Instance.CanEquip(EquipmentSlot.Accessory1, draggedEquipment))
{
  GameManager.Instance.SetEquipment(EquipmentSlot.Accessory1, draggedEquipment);
}
```

## 슬롯 타입 정의(EquipmentSlot)
- Weapon
- Armor
- Accessory1, Accessory2
- Pet1, Pet2

## GameManager와 연동이 필요한 클래스
- PlayerController : 무기 장비, 스킬 효과 반영
- PetController : 펫 장비 및 스킬("wingman") 효과 적용
- StageManager : 전투 시작 시 장비/스킬/펫 반영
- InventoryUI : 장비 선택 시 SetEquipment() 호출
- SkillSelectUI : 스킬 선택 시 AddSelectedSkill() 호출

# 마무리

GameManager는 전역에서 접근 가능한 싱글톤이며,
전투 준비부터 결과 처리까지 필요한 데이터를 안전하게 관리합니다.
팀원들은 위의 API를 통해 장비/스킬을 연동할 수 있습니다.

---
