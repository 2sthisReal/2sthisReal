using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform pivot;
    Rigidbody2D rb;
    public Player player;

    private double projectileDamage;




    private void Awake()
    {
        pivot = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }
    public void Init(Vector2 direction, float shotspeed)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.velocity = direction * shotspeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();

            if (monster != null)
            {
                CritCalculator();
                monster.TakeDamage((float)projectileDamage);
                projectileDamage = player.attackDamage;
            }

            Destroy(this.gameObject);
        }
        else if(collision.CompareTag("Wall"))
            Destroy(this.gameObject);
    }

    private void CritCalculator()
    {
        projectileDamage = player.attackDamage;
        float rand = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(rand);
        if (rand < player.critRate)
            projectileDamage = player.attackDamage * 1.5;
    }
}
