using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase Instance {  get; private set; }

    [SerializeField]
    public List<SkillData> Skills {  get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSkills();
    }

    private void LoadSkills()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "skills.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[SkillDatabase] File not found: {path}");
            Skills = new List<SkillData>();
            return;
        }

        string json = File.ReadAllText(path);
        Skills = JsonListWrapper<SkillData>.FromJson(json);

        Debug.Log($"[SkillDatabase] Loaded {Skills.Count} skills");
    }
}
