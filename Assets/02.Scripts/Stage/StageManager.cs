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
        [SerializeField] GameObject[] monsterPrefab;
        [SerializeField] GameObject[] obstaclePrefab;
        [SerializeField] GameObject[] itemPrefab;

        void Awake()
        {
            GameManager.Instance.stageManager = this;
        }

        // 스테이지 설정 세팅 및 스테이지 생성
        public void Init(GameManager gameManager)
        {
            this.gameManager = gameManager;
            stagePresetManager = GetComponentInChildren<StagePresetManager>();

            GenerateNewStage();
        }

        // 새로운 스테이지 생성
        public void GenerateNewStage()
        {
            currentPreset = stagePresetManager.GetRandomPreset();
            InitStage(currentPreset, () => {});
    
            gameManager.RegisterEnemies(currentPreset.monsterPoints.Count);
        }

        // 스테이지 미리보기
        public void PreviewStage(StagePreset preset)
        {
            InitStage(preset, () => {});
        }

        // 스테이지 설정
        void InitStage(StagePreset preset, Action onCompleted)
        {
            // 기본 타일맵 생성
            Instantiate(preset.baseTileMap, Vector3.zero, Quaternion.identity);

            // 오브젝트들 생성
            SpawnObjects(preset.monsterPoints, monsterPrefab);
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
                Instantiate(prefabs[random], point, Quaternion.identity);
            }
        }
    }
}
