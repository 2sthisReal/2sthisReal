using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bow : Weapon
{

    public GameObject arrowPrefabs;
    public bool multipleShots = false;

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
    }
    public override void AttackTarget(Vector2 vector, Player player)
    {
        if (attackCooldown <= 0f)
        {
            directionVector = vector;

            attackCooldown = 1f / player.attackSpeed;
            //multipleshot


            float spreadDistance = 0.3f; // 발사 위치 간 간격
            Vector2 basePosition = weapontransform.position;
            Vector2 perpendicular = new Vector2(-vector.y, vector.x).normalized;

            if (player.multipleShots)
            {
                for (int i = -1; i <= 1; i++)
                {
                    Vector2 offset = perpendicular * spreadDistance * i;
                    Vector2 spawnPos = basePosition + offset;

                    GameObject arrow = Instantiate(arrowPrefabs, spawnPos, Quaternion.identity);
                    arrow.GetComponent<ProjectileController>().Init(vector, player.shotSpeed);
                    arrow.GetComponent<ProjectileController>().SetPlayer(player);
                }
            }
            else
            {
                GameObject arrow = Instantiate(arrowPrefabs, weapontransform.position, Quaternion.identity);
                arrow.GetComponent<ProjectileController>().Init(directionVector, player.shotSpeed);
                arrow.GetComponent<ProjectileController>().SetPlayer(player);
            }
        }
    }
}
