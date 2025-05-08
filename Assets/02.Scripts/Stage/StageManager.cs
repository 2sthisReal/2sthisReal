using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] StagePreset currentPreset;

    [SerializeField] GameObject monsterPrefab;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject itemPrefab;


    [ContextMenu("TestInitStage")]
    public void TestInitStage()
    {
        InitStage(currentPreset);
    }

    public void InitStage(StagePreset preset)
    {
        if(preset.monsterPoints != null)
        {
            foreach(var point in preset.monsterPoints)
            {
                // 몬스터 소환
                GameObject obj = Instantiate(monsterPrefab);
                obj.transform.position = point;
                obj.transform.rotation = Quaternion.identity;
            }
        }

        if(preset.obstaclePoints != null)
        {
            foreach(var point in preset.obstaclePoints)
            {
                // 장애물 소환
                GameObject obj = Instantiate(obstaclePrefab);
                obj.transform.position = point;
                obj.transform.rotation = Quaternion.identity;
            }
        }

        if(preset.itemPoints != null)
        {
            foreach(var point in preset.itemPoints)
            {
                // 아이템 소환
                GameObject obj = Instantiate(itemPrefab);
                obj.transform.position = point;
                obj.transform.rotation = Quaternion.identity;
            }
        }
    }
}
