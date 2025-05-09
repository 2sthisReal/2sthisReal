using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jang
{
    public class StagePresetTool : MonoBehaviour
    {
        public int stageID = 101;

        [SerializeField] Transform monsterPoint;
        [SerializeField] Transform obstaclePoint;
        [SerializeField] Transform itemPoint;

        public List<Transform> monsterPoints { private set; get; }
        public List<Transform> obstaclePoints { private set; get; }
        public List<Transform> itemPoints { private set; get; }

        // 각 위치들 Gizmo 표시
        void OnDrawGizmos()
        {
            monsterPoints = monsterPoint.GetComponentsInChildren<Transform>().Where(t => t != monsterPoint).ToList();
            obstaclePoints = obstaclePoint.GetComponentsInChildren<Transform>().Where(t => t != obstaclePoint).ToList();
            itemPoints = itemPoint.GetComponentsInChildren<Transform>().Where(t => t != itemPoint).ToList();

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

        void DrawPoints(List<Transform> points)
        {
            if (points == null) return;

            foreach(var point in points)
            {
                Gizmos.DrawCube(point.position, Vector3.one);    
            }
        }
    }
}
