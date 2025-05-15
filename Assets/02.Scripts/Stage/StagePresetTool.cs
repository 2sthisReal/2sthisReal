using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jang
{
    public class StagePresetTool : MonoBehaviour
    {
        [Header("Preview Settings")]
        GameObject preview;
        [SerializeField] GameObject[] monsterPrefabs;
        [SerializeField] GameObject[] obstaclePrefabs;
        [SerializeField] GameObject[] itemPrefabs;

        [Header("Editor Settings")]
        public int stageID = 101;
        public GameObject baseTileMap;
        [SerializeField] Transform monsterPoint;
        [SerializeField] Transform obstaclePoint;
        [SerializeField] Transform itemPoint;

        public bool drawGizmos = true;
        public List<Transform> monsterPoints { private set; get; }
        public List<Transform> obstaclePoints { private set; get; }
        public List<Transform> itemPoints { private set; get; }

        // 각 위치들 Gizmo 표시
        void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                GetPoints();

                // 몬스터 위치 표시
                Gizmos.color = Color.red;
                DrawPoints(monsterPoints);

                // 장애물 위치 표시
                Gizmos.color = Color.blue;
                DrawPoints(obstaclePoints);

                // 아이템 위치 표시
                Gizmos.color = Color.green;
                DrawPoints(itemPoints);
            }
        }

        void DrawPoints(List<Transform> points)
        {
            if (points == null) return;

            foreach (var point in points)
            {
                Gizmos.DrawCube(point.position, Vector3.one);
            }
        }

        void GetPoints()
        {
            monsterPoints = monsterPoint.GetComponentsInChildren<Transform>().Where(t => t != monsterPoint).ToList();
            obstaclePoints = obstaclePoint.GetComponentsInChildren<Transform>().Where(t => t != obstaclePoint).ToList();
            itemPoints = itemPoint.GetComponentsInChildren<Transform>().Where(t => t != itemPoint).ToList();
        }

        public void PreviewStage()
        {
            if(preview != null) Destroy(preview);
            
            preview = new GameObject("Preview");

            Instantiate(baseTileMap, preview.transform);

            SpawnObjects(monsterPoints, monsterPrefabs);
            SpawnObjects(obstaclePoints, obstaclePrefabs);
            SpawnObjects(itemPoints, itemPrefabs);
        }

        void SpawnObjects(List<Transform> points, GameObject[] prefabs)
        {
            if (points == null || prefabs == null) return;

            foreach (var point in points)
            {
                int random = UnityEngine.Random.Range(0, prefabs.Length);
                GameObject obj = Instantiate(prefabs[random], point.position, Quaternion.identity);
                obj.transform.SetParent(preview.transform);
            }
        }
    }
}
