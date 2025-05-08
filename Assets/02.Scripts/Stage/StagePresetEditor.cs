using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StagePresetEditor : MonoBehaviour
{
    public int stageID = 101;

    public Transform monsterPoint;
    public Transform obstaclePoint;
    public Transform itemPoint;

    public List<Transform> monsterPoints { private set; get; }
    public List<Transform> obstaclePoints { private set; get; }
    public List<Transform> itemPoints { private set; get; }

    // 각 위치들 Gizmo 표시
    void OnDrawGizmos()
    {
        monsterPoints = monsterPoint.GetComponentsInChildren<Transform>().Where(t => t != monsterPoint).ToList();
        obstaclePoints = obstaclePoint.GetComponentsInChildren<Transform>().Where(t => t != obstaclePoint).ToList();
        itemPoints = itemPoint.GetComponentsInChildren<Transform>().Where(t => t != itemPoint).ToList();

        Gizmos.color = Color.red;
        foreach (var point in monsterPoints)
            if (point != null) Gizmos.DrawCube(point.position, Vector3.one);

        Gizmos.color = Color.blue;
        foreach (var point in obstaclePoints)
            if (point != null) Gizmos.DrawCube(point.position, Vector3.one);
        Gizmos.color = Color.green;
        foreach (var point in itemPoints)
            if (point != null) Gizmos.DrawCube(point.position, Vector3.one);
    }
}
