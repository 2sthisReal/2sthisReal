using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Jang
{
    [CustomEditor(typeof(StagePresetTool))]
    public class StagePresetToolEditor : Editor
    {
        const string PATH = "Assets/Resources/StagePresets/";
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // 기본 인스펙터 그리기

            StagePresetTool tool = (StagePresetTool)target;

            if (GUILayout.Button("Save New StagePreset")) // Save StagePreset 버튼 생성
            {
                SaveNewPreset(tool); // 버튼을 누르면 SavePreset 메서드 실행
            }

            if (GUILayout.Button("Overwrite StagePreset"))
            {
                OverwritePreset(tool);
            }

            if(GUILayout.Button("Preview Preset"))
            {
                PreviewPreset(tool);
            }
        }

        // 프리셋 저장
        private void SaveNewPreset(StagePresetTool tool)
        {
            // ScriptableObject 생성
            StagePreset preset = CreateInstance<StagePreset>();

            // 프리셋 ID, 오브젝트들 위치 저장
            SaveDataToPreset(tool, preset);

            string path = $"{PATH}Stage_{tool.stageID}.asset";

            // 같은 ID의 프리셋이 존재한다면 생성 X
            if (AssetDatabase.LoadAssetAtPath<StagePreset>(path) != null)
            {
                Debug.LogWarning($"{tool.stageID} is Already Exsit StageID");
                return;
            }

            // 프리셋 생성
            AssetDatabase.CreateAsset(preset, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"Generate New StagePreset {tool.stageID}. path: {path}");
        }

        // 프리셋 덮어쓰기
        private void OverwritePreset(StagePresetTool tool)
        {
            string path = $"{PATH}Stage_{tool.stageID}.asset";
            StagePreset exist = AssetDatabase.LoadAssetAtPath<StagePreset>(path);

            if (exist == null)
            {
                Debug.LogWarning("Doesn't Exist Preset to Overwirte");
                return;
            }

            if (!EditorUtility.DisplayDialog("Check Overwrite", $"Overwrite {tool.stageID} Preset?", "Yes", "No"))
            {
                return;
            }


            // 새로운 임시 ScriptableObject를 만들어 데이터 채움
            StagePreset temp = CreateInstance<StagePreset>();

            SaveDataToPreset(tool, temp);

            // 기존 프리셋에 덮어쓰기
            EditorUtility.CopySerialized(temp, exist);
            EditorUtility.SetDirty(exist);
            AssetDatabase.SaveAssets();

            Debug.Log($"StagePreset(ID: {tool.stageID})is Overwrited");
        }

        // 데이터 프리셋에 적용
        private void SaveDataToPreset(StagePresetTool tool, StagePreset preset)
        {
            // 프리셋 ID 설정
            preset.stageID = tool.stageID;
            preset.name = $"Stage_{tool.stageID}";
            preset.baseTileMap = tool.baseTileMap;

            // 각 몬스터, 장애물, 아이템 위치 저장
            foreach (var point in tool.monsterPoints)
                if (point != null) preset.monsterPoints.Add(point.position);

            foreach (var point in tool.obstaclePoints)
                if (point != null) preset.obstaclePoints.Add(point.position);

            foreach (var point in tool.itemPoints)
                if (point != null) preset.itemPoints.Add(point.position);
        }

        // 프리셋 미리보기
        private void PreviewPreset(StagePresetTool tool)
        {
            StagePreset[] presets = Resources.LoadAll<StagePreset>("StagePresets");
            StagePreset preset = presets.SingleOrDefault(p => p.stageID == tool.stageID);

            if(preset == null)
            {
                Debug.LogWarning($"Doesn't Exist Preset (ID: {tool.stageID})");
                return;
            }

            tool.stageManager.PreviewStage(preset);
        }
    }
}
