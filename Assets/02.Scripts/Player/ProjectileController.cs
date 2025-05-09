using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform pivot;
    Vector2 direction;
    float shotSpeed;
    Rigidbody2D rb;
    private void Awake()
    {
        pivot = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(Vector2 direction, float shotspeed)
    {
        this.direction = direction;
        this.shotSpeed = shotspeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);


        rb.velocity = this.direction * shotSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Dealt Damage


            Destroy(this.gameObject);
        }
    }
}
