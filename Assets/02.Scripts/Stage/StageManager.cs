using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jang
{
    public class StageManager : MonoBehaviour
    {
        GameManager gameManager;
        [SerializeField] StagePreset currentPreset; // 현재 스테이지 프리셋
        [SerializeField] StagePreset bossStagePreset;
        private StagePresetManager stagePresetManager; // 프리셋 랜덤으로 가져오기 위한 Manager
        [SerializeField] FadeScreenController fadeScreenController;
        [SerializeField] Transform player;
        [SerializeField] Vector2 startPos;

        [Header("Object Prefabs")]
        [SerializeField] MonsterSpawner monsterSpawner;
        [SerializeField] GameObject[] obstaclePrefab;
        [SerializeField] GameObject[] itemPrefab;
        [SerializeField] GameObject stageMap;
        private bool isClear = false;
        public int CurrentStage { get; private set; }
        [SerializeField] StageDoor stageDoor;

        [SerializeField] int bossStage;
        public static event Action<int> onStageStart;

        // 스테이지 설정 세팅 및 스테이지 생성
        public void Init(GameManager gameManager)
        {
            this.gameManager = gameManager;
            stagePresetManager = GetComponentInChildren<StagePresetManager>();
            monsterSpawner = GetComponentInChildren<MonsterSpawner>();
            stageDoor = FindObjectOfType<StageDoor>();
            player = FindObjectOfType<Player>().transform;

            CurrentStage = 1;
            monsterSpawner.CurrentStage = CurrentStage;
            
            StartStage();
        }

        public void StartStage()
        {
            StartCoroutine(StartStageCorutine());
        }

        private IEnumerator StartStageCorutine()
        {
            if (CurrentStage == 1)
                fadeScreenController.SetBlackScreen();
            else
                yield return fadeScreenController.FadeOut();

            // 스테이지 세팅
            if (CurrentStage == bossStage)
            {
                stageDoor.gameObject.SetActive(false);
                GenerateBossStage();
            }
            else
            {
                stageDoor.SetDoor(isClear);
                GenerateNewStage();
            }

            player.position = startPos;
            onStageStart?.Invoke(CurrentStage);
            yield return fadeScreenController.FadeIn();
        }

        [ContextMenu("StageClear")]
        public void StageClear()
        {
            isClear = true;
            CurrentStage++;
            stageDoor.SetDoor(isClear);

            monsterSpawner.CurrentStage = CurrentStage;
        }

        // 새로운 스테이지 생성
        void GenerateNewStage()
        {
            isClear = false;
            stageDoor.SetDoor(isClear);

            // 일반 스테이지 설정
            monsterSpawner.SetBossStage(false);

            currentPreset = stagePresetManager.GetRandomPreset();
            InitStage(currentPreset);

            gameManager.RegisterEnemies(currentPreset.monsterPoints.Count);
        }

        void GenerateBossStage()
        {
            Camera.main.GetComponent<FollowCamera>().enabled = true;
            isClear = false;

            // 보스 스테이지 설정
            monsterSpawner.SetBossStage(true);

            InitStage(bossStagePreset);

            gameManager.RegisterEnemies(1);  // 보스는 한 마리만 등록
        }

        // 스테이지 설정
        void InitStage(StagePreset preset)
        {
            // 기존 스테이지 맵 정리
            if (stageMap != null)
            {
                Destroy(stageMap);
            }

            GameObject map = new GameObject("Stage");
            stageMap = map;

            // 기본 타일맵 생성
            Instantiate(preset.baseTileMap, stageMap.transform);

            // 오브젝트들 생성
            SpawnMonster(preset.monsterPoints);
            SpawnObjects(preset.obstaclePoints, obstaclePrefab);
            SpawnObjects(preset.itemPoints, itemPrefab);
        }

        // 오브젝트 생성
        void SpawnObjects(List<Vector2> points, GameObject[] prefabs)
        {
            if (points == null || prefabs == null) return;

            foreach (var point in points)
            {
                int random = UnityEngine.Random.Range(0, prefabs.Length);
                GameObject obj = Instantiate(prefabs[random], point, Quaternion.identity);
                obj.transform.SetParent(stageMap.transform);
            }
        }

        void SpawnMonster(List<Vector2> points)
        {
            if (points == null) return;

            foreach (var point in points)
            {
                monsterSpawner.SpawnMonsterInStage(point);
            }
        }
    }
}