using UnityEngine;
using System;

[Serializable]
public class EquipmentData
{
    public string equipmentID;
    public string equipmentName;

    public float bonusAttack;
    public float bonusHealth;

    public string iconPath;

    public EquipmentType equipmentType;
}