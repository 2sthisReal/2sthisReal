using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StagePreset", menuName = "Stage/StagePreset")]
public class StagePreset : ScriptableObject
{
    public int stageID; // 스테이지 프리셋 아이디
    public GameObject baseTileMap;
    public List<Vector2> monsterPoints = new(); // 몬스터가 생성될 위치
    public List<Vector2> obstaclePoints = new(); // 장애물이 생성될 위치
    public List<Vector2> itemPoints = new(); // 아이템이 생성될 위치
}