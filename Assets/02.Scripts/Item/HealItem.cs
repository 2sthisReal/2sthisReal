using System;
using UnityEngine;

public class HealItem : ItemBase
{
    [SerializeField] int value = 10;

    protected override void UseItem(BaseCharacter player)
    {
        player.currentHealth += value;
    }
}
