using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EquipmentDatabase : MonoBehaviour
{
    public List<EquipmentData> Weapons { get; private set; }
    public List<EquipmentData> Armors { get; private set; }
    public List<EquipmentData> Accessories { get; private set; }
    public List<EquipmentData> AllEquipments { get; private set; }

    private void Awake()
    {
        Weapons = LoadEquipmentList("Equiments/weapons.json");
        Armors = LoadEquipmentList("Equipments/armors.json");
        Accessories = LoadEquipmentList("Equipments/accessories.json");

        AllEquipments = Weapons.Concat(Armors).Concat(Accessories).ToList();

        Debug.Log($"[EquipmentDatabase] Loaded {AllEquipments.Count} equipments");
    }

    private List<EquipmentData> LoadEquipmentList(string relativePath)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"Equipment file not found: {fullPath}");
            return new List<EquipmentData> ();
        }

        string json = File.ReadAllText(fullPath);
        return JsonListWrapper<EquipmentData>.FromJson(json);
    }

}
