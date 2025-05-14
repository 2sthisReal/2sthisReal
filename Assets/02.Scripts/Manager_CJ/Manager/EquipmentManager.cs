using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager
{
    private readonly Dictionary<EquipmentSlot, EquipmentData> equippedSlots = new();

    public void Set(EquipmentSlot slot, EquipmentData equipment)
    {
        equippedSlots[slot] = equipment;
    }

    public EquipmentData Get(EquipmentSlot slot)
    {
        return equippedSlots.TryGetValue(slot, out var equipment) ? equipment : null;
    }

    public Dictionary<EquipmentSlot, EquipmentData> GetAll()
    {
        return new Dictionary<EquipmentSlot, EquipmentData>(equippedSlots);
    }

    public void AutoAssign(EquipmentData equipment)
    {
        switch (equipment.equipmentType)
        {
            case EquipmentType.Weapon:
                Set(EquipmentSlot.Weapon, equipment);
                break;
            case EquipmentType.Armor:
                Set(EquipmentSlot.Armor, equipment);
                break;
            case EquipmentType.Accessory:
                if (!equippedSlots.ContainsKey(EquipmentSlot.Accessory1))
                    Set(EquipmentSlot.Accessory1, equipment);
                else
                    Set(EquipmentSlot.Accessory2, equipment);
                break;
            case EquipmentType.Pet:
                if (!equippedSlots.ContainsKey(EquipmentSlot.Pet1))
                    Set(EquipmentSlot.Pet1, equipment);
                else
                    Set(EquipmentSlot.Pet2, equipment);
                break;
        }
    }
}
