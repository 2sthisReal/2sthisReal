using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StagePresetEditor))]
public class StageEditorInspector : Editor
{
    const string PATH = "Assets/Resources/StagePresets/";
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StagePresetEditor editor = (StagePresetEditor)target;

        if(GUILayout.Button("Save StagePreset"))
        {
            SavePreset(editor);
        }
    }

    // 프리셋 저장
    private void SavePreset(StagePresetEditor editor)
    {
        // ScriptableObject 생성
        StagePreset preset = CreateInstance<StagePreset>();

        // 스테이지 아이디 저장
        preset.stageID = editor.stageID;

        // 각 몬스터, 장애물, 아이템 위치 저장
        foreach(var point in editor.monsterPoints)
            if(point != null) preset.monsterPoints.Add(point.position);
        
        foreach(var point in editor.obstaclePoints)
            if(point != null) preset.obstaclePoints.Add(point.position);

        foreach(var point in editor.itemPoints)
            if(point != null) preset.itemPoints.Add(point.position);

        string path = $"{PATH}Stage_{editor.stageID}.asset";

        // 같은 ID의 프리셋이 존재한다면 생성 X
        if(AssetDatabase.LoadAssetAtPath<StagePreset>(path) != null)
        {
            Debug.LogWarning("이미 존재하는 ID 입니다.");
            return;
        }

        // 프리셋 생성
        AssetDatabase.CreateAsset(preset, path);
        AssetDatabase.SaveAssets();

        Debug.Log($"스테이지 프리셋이 생성되었습니다. 저장 경로: {path}");
    }
}
