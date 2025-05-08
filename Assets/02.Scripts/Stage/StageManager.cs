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
            if (preset.monsterPoints != null)
            {
                foreach (var point in preset.monsterPoints)
                {
                    // 몬스터 소환
                    GameObject obj = Instantiate(monsterPrefab);
                    obj.transform.position = point;
                    obj.transform.rotation = Quaternion.identity;
                }
            }

            if (preset.obstaclePoints != null)
            {
                foreach (var point in preset.obstaclePoints)
                {
                    // 장애물 소환
                    GameObject obj = Instantiate(obstaclePrefab);
                    obj.transform.position = point;
                    obj.transform.rotation = Quaternion.identity;
                }
            }

            if (preset.itemPoints != null)
            {
                foreach (var point in preset.itemPoints)
                {
                    // 아이템 소환
                    GameObject obj = Instantiate(itemPrefab);
                    obj.transform.position = point;
                    obj.transform.rotation = Quaternion.identity;
                }
            }
        }
    }
}
