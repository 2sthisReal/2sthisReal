using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : BaseCharacter
{
    Animator weaponAnimator;
    SpriteRenderer spriteRenderer;
    Transform playerTransform;
    public Weapon weaponbow;
    Transform closest;

    public bool inRanged;


    //추상클래스에서 스탯 따옴
    [SerializeField] float tempspeed = 3;   //플레이어의 속도
    List<Transform> monsterCounter = new List<Transform>();


    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.Find("Sprite").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon_bow").GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
    }


    private void FixedUpdate()
    {
        Move(Vector2.zero);
    }


    void Update()
    {
        monsterCounter.RemoveAll(monster => monster == null);
        closest = TargetSet();

        if (closest == null)
        {
            weaponbow.WeaponWait();
            return;
        }
        weaponbow.WeaponReady();
        weaponbow.GetVector((closest.position-playerTransform.position).normalized);
        if (closest.position.x - playerTransform.position.x > 0)
            spriteRenderer.flipX = false;    //플레이어가 오른쪽을 바라보게 함
        else if (closest.position.x - playerTransform.position.x < 0)
            spriteRenderer.flipX = true;   //플레이어가 왼쪽을 바라보게 함
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        monsterCounter.Add(collision.transform);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 targetPos = collision.transform.position;

        //attack 방향벡터 설정
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        monsterCounter.Remove(collision.transform);
    }


    public override void Move(Vector2 vector)
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = (new Vector2(moveHorizontal, moveVertical).normalized) * tempspeed;
        rb.velocity = movement;
        animator.SetBool("IsMoving", movement.magnitude > 0.5f);
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
    public override void Attack()
    {
        //
        Debug.Log("attack");
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

}
