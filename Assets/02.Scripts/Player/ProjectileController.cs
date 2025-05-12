using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform pivot;
    Rigidbody2D rb;
    private void Awake()
    {
        pivot = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();
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
                // 체력 5 감소
                monster.currentHealth -= 5;
            }

            Destroy(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
}
