using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bow : Weapon
{

    public GameObject arrowPrefabs;


    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            attackCooldown = 1f / rate;

            GameObject arrow = Instantiate(arrowPrefabs, weapontransform.position, Quaternion.identity);
            arrow.GetComponent<ProjectileController>().Init(directionVector, shotSpeed);
        }
    }
    public void AttackTarget(Vector2 vector)
    {
        directionVector = vector;
    }
}
