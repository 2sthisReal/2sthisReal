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
        pivot = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Vector2 direction, float shotspeed)
    {
        this.direction = direction;
        this.shotSpeed = shotspeed;
        pivot.right = this.direction;
        if (direction.x < 0)
            pivot.localRotation = Quaternion.Euler(180, 0, 0); 
        else
            pivot.localRotation = Quaternion.Euler(0, 0, 0); 
        rb.velocity = direction * shotSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //¸ó½ºÅÍ¿Í ºÎµúÈú¶§, º®°ú ºÎµúÈú‹š
        if (collision.collider.CompareTag("Enemy"))
        {
            //Dealt Damage


            Destroy(this.gameObject);
        }

    }
}
