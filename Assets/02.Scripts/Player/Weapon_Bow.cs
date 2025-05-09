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
    public void TargetVector(Vector2 vector)
    {

        if (attackCooldown <= 0f)
        {
            attackCooldown = 1f / rate;

            GameObject arrow = Instantiate(arrowPrefabs, weapontransform.position, Quaternion.identity);
            arrow.GetComponent<ProjectileController>().Init(directionVector, shotSpeed);
        }
    }
}
