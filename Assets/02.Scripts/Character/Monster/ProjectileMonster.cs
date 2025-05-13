using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMonster : Monster
{
    private float attackCooldownTimer;

    protected override void Update()
    {
        base.Update();

        if (!isAlive || player == null || monsterData.attackPattern != AttackPattern.Ranged)
            return;

        float distance = Vector2.Distance(transform.position, player.position);
        attackCooldownTimer -= Time.deltaTime;

        if (distance <= attackRange && attackCooldownTimer <= 0f)
        {
            Attack();
            attackCooldownTimer = 1f / attackSpeed;
        }
    }

    public override void Attack()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Vector2 dir = (player.position - firePoint.position).normalized;
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            proj.GetComponent<Projectile>().Initialize(dir, attackDamage);
        }
    }
}

