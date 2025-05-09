using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator spriteAnimator;
    Animator weaponAnimator;
    SpriteRenderer spriteRenderer;
    Transform playerTransform;
    Weapon weapon;


    public Weapon[] weaponPrefabs;
    private Weapon currentWeapon;
    private Transform weapons;

    public bool inRanged;
    Vector2 directionVector;

    [SerializeField]float tempspeed = 3;   
    List<Transform> monsterCounter = new List<Transform>();
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
        weapon = GetComponentInChildren<Weapon>();
    }
    private void Start()
    {
        weapons = transform.Find("Weapons");
        EquipWeapon(0);
        weaponAnimator = transform.Find("Weapons").GetComponent<Animator>();

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
            weapon.WeaponWait();
            return;
        }
        weapon.WeaponReady();
        directionVector = (closest.position - playerTransform.position).normalized;
        weapon.GetVector(directionVector);
        
        if (closest.position.x - playerTransform.position.x > 0)
            spriteRenderer.flipX = false;  
        else if (closest.position.x - playerTransform.position.x < 0)
            spriteRenderer.flipX = true;   

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            monsterCounter.Add(collision.transform);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            weapon.AttackTarget(directionVector);
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

    public void EquipWeapon(int weaponIndex)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);  // 현재 장착된 무기 제거
        }

        // 새로운 무기 인스턴스를 장착
        currentWeapon = Instantiate(weaponPrefabs[weaponIndex], weapons.position, Quaternion.identity);
        currentWeapon.transform.SetParent(weapons);
        weapon = currentWeapon;
    }
}
