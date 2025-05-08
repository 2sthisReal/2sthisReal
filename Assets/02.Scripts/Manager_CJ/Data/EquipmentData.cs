using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game Data/Item")]
public class EquipmentData : ScriptableObject
{
    public string equipmentName;
    public float bonusAttack;
    public float bonusHealth;
    public Sprite icon;
    public EquipmentType equipmentType;
}

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory
}
