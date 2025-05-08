using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PetDatabase : MonoBehaviour
{
    public List<PetData> Pets { get; private set; }

    private void Awake()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "pets.json");
        string json = File.ReadAllText(path);
        Pets = JsonListWrapper<PetData>.FromJson(json);

        Debug.Log($"Loaded {Pets.Count} pets.");
    }
}
