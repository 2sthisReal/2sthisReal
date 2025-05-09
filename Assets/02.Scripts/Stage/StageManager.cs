using System.Collections.Generic;
using UnityEngine;

namespace Jang
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] StagePreset currentPreset; // 현재 스테이지 프리셋
        private StagePresetManager stagePresetManager; // 프리셋 랜덤으로 가져오기 위한 Manager

        [Header("Object Prefabs")]
        [SerializeField] GameObject monsterPrefab; 
        [SerializeField] GameObject obstaclePrefab;
        [SerializeField] GameObject itemPrefab;


        public void Init()
        {
            if (stagePresetManager == null)
                stagePresetManager = GetComponentInChildren<StagePresetManager>();

            currentPreset = stagePresetManager.GetRandomPreset();
            InitStage(currentPreset);
        }

        
        [ContextMenu("TestInitStage")]
        public void TestInitStage()
        {
            InitStage(currentPreset);
        }
        
        // 스테이지 설정
        void InitStage(StagePreset preset)
        {
            SpawnObjects(preset.monsterPoints, monsterPrefab);
            SpawnObjects(preset.obstaclePoints, obstaclePrefab);
            SpawnObjects(preset.itemPoints, itemPrefab);
        }

        // 오브젝트 생성
        void SpawnObjects(List<Vector2> points, GameObject prefab)
        {
            if(points == null || prefab == null) return;

            foreach(var point in points)
            {
                GameObject obj = Instantiate(prefab, point, Quaternion.identity);
            }
        }
    }
}
