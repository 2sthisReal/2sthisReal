using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator spriteAnimator;
    Animator weaponAnimator;
    SpriteRenderer spriteRenderer;
    Transform playerTransform;
    Weapon_Bow weaponbow;

    public bool inRanged;
    Vector2 directionVector;

    [SerializeField]float tempspeed = 3;   
    List<Transform> monsterCounter = new List<Transform>();
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon_bow").GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
        weaponbow = GetComponentInChildren<Weapon_Bow>();
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }

    void Update()
    {
        monsterCounter.RemoveAll(monster => monster == null);
        Transform closest = TargetSet();
        if (closest == null)
        {
            weaponbow.WeaponWait();
            weaponbow.StopAttack();
            return;
        }
        weaponbow.WeaponReady();
        directionVector = (closest.position - playerTransform.position).normalized;
        weaponbow.GetVector(directionVector);
        
        if (closest.position.x - playerTransform.position.x > 0)
            spriteRenderer.flipX = false;  
        else if (closest.position.x - playerTransform.position.x < 0)
            spriteRenderer.flipX = true;   

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        monsterCounter.Add(collision.transform);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            weaponbow.AttackTarget(directionVector);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        monsterCounter.Remove(collision.transform);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        if (circle != null)
        {
            Vector3 position = transform.position + (Vector3)circle.offset;
            Gizmos.DrawWireSphere(position, circle.radius);
        }
    }


    void PlayerMove()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = (new Vector2(moveHorizontal, moveVertical).normalized) * tempspeed;
        rb.velocity = movement;
        spriteAnimator.SetBool("IsMoving", movement.magnitude > 0.5f);
    }

    Transform TargetSet()
    {
        Transform closest = null;
        float minDistant = float.MaxValue;

        foreach (var t in monsterCounter)
        {
            float distant = (t.position - transform.position).sqrMagnitude;
            if (distant < minDistant)
            {
                minDistant = distant;
                closest = t;
            }
        }

        return closest;
    }
}
