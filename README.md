# 🛡️ Legend of Knight - 기사의 전설

Unity 기반 로그라이크 액션 게임 프로젝트

---

## 📌 프로젝트 개요

**기사의 전설**은 1인 플레이 로그라이크 형식의 Unity 입문 프로젝트로, 플레이어가 스테이지를 클리어하며 다양한 보상과 랜덤 요소를 경험할 수 있도록 구성된 게임입니다.

- **플레이어 조작** 및 **적 처치**
- **스킬 및 아이템 장착 시스템**
- **랜덤 스테이지 생성**
- **클래스 기반 객체지향 설계**
- **UI, Sound, Animator, Preset 시스템 도입**

---

## ⚙️ 기술 스택

| 구성 요소       | 설명 |
|----------------|------|
| Unity          | 게임 엔진 |
| C#             | 게임 로직 및 시스템 구현 |
| Visual Studio  | 개발 IDE |
| GitHub         | 협업 및 버전 관리 |
| JSON           | Preset 및 데이터 저장 |

---

## 🧱 주요 구조

### Game Systems
- `GameManager` - 전역 데이터 관리 및 인터페이스 제공
- `StageManager` - 스테이지 로딩 및 관리
- `SkillManager`, `EquipmentManager` - 능력치/아이템 시스템
- `UIManager` - 게임 UI 전반 담당
- `SceneLoader` - 씬 전환 관리
- `MonsterManager`, `Player` - 전투 요소 구현
- `SoundManager` - 사운드 처리

### 데이터
- `StagePresetManager`: 프리셋 기반 스테이지 정의
- `EquipmentDatabase`, `SkillDatabase`: JSON 기반 데이터 정의

---

## 🧑‍🤝‍🧑 팀 구성 및 역할

| 이름     | 담당 업무 |
|----------|-----------|
| 김선우   | UI 설계 및 구현, 결과 보고서, 팀장 |
| 장민제   | 몬스터 구현, Preset Editor |
| 김성주   | 플레이어 이동 및 공격 |
| 공재원   | GameManager, 데이터 구조 설계 |
| 이창주   | 스킬/아이템 시스템 및 설계 |

---

## 📅 개발 일정

| 기간          | 내용 |
|---------------|------|
| 05/08         | 기획 및 와이어프레임 |
| 05/09~05/12   | 필수 기능 구현 |
| 05/12~05/13   | 추가 기능 구현 |
| 05/14         | 최종 테스트 |
| 05/15         | 문서화 및 발표 자료 작성 |

---

## 🐞 트러블슈팅 하이라이트

- **UI가 비활성화 상태일 때 OnSceneLoaded 작동 안함** → `OnEnable`로 이전
- **발사체가 자기 자신을 인식하여 방향이 틀어짐** → 충돌 레이어 설정으로 해결
- **Animator와 SpriteRenderer 충돌** → SpriteRenderer 직접 제어 제거, Animator에 위임
- **몬스터 미생성** → 좌표 데이터 누락 / null 체크 추가
- **GameManager 확장성 부족** → 구조 리팩토링 및 인터페이스 재설계

---

## 📝 개선 사항 및 회고

- UI/UX 요소 강화 필요 (타격감, 시각 효과 등)
- 스킬 시스템의 클래스 설계 및 유연성 개선 필요
- 불필요한 코드 정리 및 공통 요소 통합 필요
- 협업 중 깃 충돌 예방을 위한 관리 강화 필요

---

## 🔗 기타

- 프로젝트는 Unity 입문자들의 실습을 위한 목적이며, 구조 설계 및 코드 협업의 기초에 중점을 두었습니다.
- 전체 구조는 객체지향적 방식과 인터페이스 기반으로 구성되며, 확장 가능성을 염두에 두고 설계되었습니다.

---

## 🎮 실행 방법

1. Unity Hub에서 본 프로젝트 열기
2. `MainScene` 또는 `StartScene` 실행
3. 게임 플레이

---

## 🎨 스킬 아이콘 출처 (CC BY 3.0)

- **MultiShot**  
  [Striking arrows icon](https://game-icons.net/1x1/lorc/striking-arrows.html) by **Lorc**, licensed under [CC BY 3.0](https://creativecommons.org/licenses/by/3.0/)

- **Reflection**  
  [Divert icon](https://game-icons.net/1x1/lorc/divert.html) by **Lorc**, licensed under [CC BY 3.0](https://creativecommons.org/licenses/by/3.0/)

- **Pierce**  
  [Pierced body icon](https://game-icons.net/1x1/lorc/pierced-body.html) by **Lorc**, licensed under [CC BY 3.0](https://creativecommons.org/licenses/by/3.0/)

- **AttackUp**  
  [Crossed swords icon](https://game-icons.net/1x1/lorc/crossed-swords.html) by **Lorc**, licensed under [CC BY 3.0](https://creativecommons.org/licenses/by/3.0/)

- **AttackSpeed**  
  [Sword brandish icon](https://game-icons.net/1x1/delapouite/sword-brandish.html) by **Delapouite**, licensed under [CC BY 3.0](https://creativecommons.org/licenses/by/3.0/)

- **MaxHealth**  
  [Health increase icon](https://game-icons.net/1x1/sbed/health-increase.html) by **sbed**, licensed under [CC BY 3.0](https://creativecommons.org/licenses/by/3.0/)

- **MonsterBounce**  
  [Tron arrow icon](https://game-icons.net/1x1/lorc/tron-arrow.html) by **Lorc**, licensed under [CC BY 3.0](https://creativecommons.org/licenses/by/3.0/)

- **OrbitSword**  
  [Sword spin icon](https://game-icons.net/1x1/lorc/sword-spin.html) by **Lorc**, licensed under [CC BY 3.0](https://creativecommons.org/licenses/by/3.0/)

