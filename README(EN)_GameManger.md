# GameManager

This document explains how to use the GameManager, which handles game state transitions and manages player-related data such as skills, equipment, and pets.

---

## Responsibilities (Implemented)

- Game state transitions (Main Menu → In-Game → Stage Clear, etc.)
- Storage of selected skills, equipment, and pets
- Enemy count tracking during combat and automatic stage clear handling
- Session reset on Game Over / Victory

---

## GameManager Structure Overview

| Property | Description |
|----------|-------------|
| `SelectedSkills` | List of selected skills |
| `EquippedSlots` | Equipment slots: Weapon, Armor, Accessory x2, Pet x2 |
| `GameState` | Current game state (`MainMenu`, `Preparing`, `InGame`, `StageClear`, etc.) |
| `remainingEnemies` | Number of enemies remaining; stage is cleared when it reaches `0` |

---

## How to Use GameManager in Other Systems

### Equipment

```csharp
var weapon = GameManager.Instance.GetEquipment(EquipmentSlot.Weapon);
if (weapon != null)
    player.ApplyWeapon(weapon);

var pet1 = GameManager.Instance.GetEquipment(EquipmentSlot.Pet1);
```

---

### Skills
```csharp
foreach (var skill in GameManager.Instance.GetSelectedSkills())
{
    Debug.Log(skill.skillName);
}

if (GameManager.Instance.HasSkill("wingman"))
{
    petController.EnableProjectileBlocking();
}
```

---

### Reset on Session End
```csharp
GameManager.Instance.ResetPlayerSession();
```

---

### Equipment Drag-and-Drop Example
```csharp
if (GameManager.Instance.CanEquip(EquipmentSlot.Accessory1, draggedEquipment))
{
    GameManager.Instance.SetEquipment(EquipmentSlot.Accessory1, draggedEquipment);
}
```
---

## EquipmentSlot Type

- Weapon
- Armor
- Accessory1, Accessory2
- Pet1, Pet2

Use CanEquip() to validate slot-type compatibility.

---

## Classes That Should Use GameManager

- PlayerController: Applies equipped weapon and skill effects
- PetController: Applies pet equipment and "wingman" skill
- StageManager: Initializes selected gear/skills/pets at battle start
- InventoryUI: Calls SetEquipment() on item drop
- SkillSelectUI: Calls AddSelectedSkill() on skill selection

---

# Summary

The GameManager acts as the central hub for game session data and state transitions.
It provides safe, global access to selected skills, equipped items, and pets.
Other systems can query and apply this data using the public API shown above.
