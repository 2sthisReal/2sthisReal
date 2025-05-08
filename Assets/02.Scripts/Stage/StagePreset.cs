using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StagePreset", menuName = "Stage/StagePreset")]
public class StagePreset : ScriptableObject
{
    public int stageID;
    public List<Vector2> monsterPoints = new();
    public List<Vector2> obstaclePoints = new();
    public List<Vector2> itemPoints = new();
    
}
