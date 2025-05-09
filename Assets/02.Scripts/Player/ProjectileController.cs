using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform pivot;
    Vector2 direction;
    float shotSpeed;
    Rigidbody2D rigidbody;
    private void Awake()
    {
        pivot = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
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
        rigidbody.velocity = direction * shotSpeed;
    }
}
