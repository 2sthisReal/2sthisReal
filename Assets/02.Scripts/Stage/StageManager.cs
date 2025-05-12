using System;
using System.Collections.Generic;
using UnityEngine;


namespace Jang
{
    public class StageManager : MonoBehaviour
    {
        GameManager gameManager;
        [SerializeField] StagePreset currentPreset; // 현재 스테이지 프리셋
        private StagePresetManager stagePresetManager; // 프리셋 랜덤으로 가져오기 위한 Manager

        [Header("Object Prefabs")]
        [SerializeField] MonsterSpawner monsterSpawner;
        [SerializeField] GameObject[] obstaclePrefab;
        [SerializeField] GameObject[] itemPrefab;
        [SerializeField] GameObject stageMap;
        private bool isClear = false;
        private int currentStage = 1;
        [SerializeField] StageDoor stageDoor;
        

        // 스테이지 설정 세팅 및 스테이지 생성
        public void Init(GameManager gameManager)
        {
            this.gameManager = gameManager;
            stagePresetManager = GetComponentInChildren<StagePresetManager>();
            monsterSpawner = GetComponentInChildren<MonsterSpawner>();
            stageDoor = FindObjectOfType<StageDoor>();
            

            StartStage();
        }

        public void StartStage()
        {
            stageDoor.SetDoor(isClear);
            GenerateNewStage();

            currentStage = 1;
        }

        [ContextMenu("StageClear")]
        public void StageClear()
        {
            isClear = true;
            currentStage++;
            stageDoor.SetDoor(isClear);
        }

        // 새로운 스테이지 생성
        void GenerateNewStage()
        {
            isClear = false;
            stageDoor.SetDoor(isClear);

            currentPreset = stagePresetManager.GetRandomPreset();
            InitStage(currentPreset, () => { });

            gameManager.RegisterEnemies(currentPreset.monsterPoints.Count);
        }

        // 스테이지 미리보기
        public void PreviewStage(StagePreset preset)
        {
            InitStage(preset, () => { });
        }

        // 스테이지 설정
        void InitStage(StagePreset preset, Action onCompleted)
        {
            Destroy(stageMap);
            stageMap = new GameObject("Stage");

            // 기본 타일맵 생성
            Instantiate(preset.baseTileMap, Vector3.zero, Quaternion.identity);

            // 오브젝트들 생성
            SpawnMonster(preset.monsterPoints);
            SpawnObjects(preset.obstaclePoints, obstaclePrefab);
            SpawnObjects(preset.itemPoints, itemPrefab);

            onCompleted?.Invoke();
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

        void SpawnMonster(List<Vector2>points)
        {
            if(points == null) return;

            foreach(var point in points)
            {
                monsterSpawner.SpawnMonsterInStage(point);
            }
        }
    }
}
