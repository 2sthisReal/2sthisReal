using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bow : Weapon
{

    public GameObject arrowPrefabs;


    private void Update()
    {
        attackCooldown -= Time.deltaTime;
    }
    public override void AttackTarget(Vector2 vector, float ShotSpeed, float Rate)
    {
        if (attackCooldown <= 0f)
        {
            directionVector = vector;

            attackCooldown = 1f / Rate;

            GameObject arrow = Instantiate(arrowPrefabs, weapontransform.position, Quaternion.identity);
            arrow.GetComponent<ProjectileController>().Init(directionVector, ShotSpeed);
        }
    }
}
