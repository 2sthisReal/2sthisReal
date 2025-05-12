using SWScene;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : BaseCharacter
{
    Animator weaponAnimator;
    SpriteRenderer spriteRenderer;
    Transform playerTransform;
    Weapon weapon;


    public Weapon[] weaponPrefabs;
    private Weapon currentWeapon;
    private Transform weapons;

    public float shotSpeed;

    private bool isMove;
    public bool inRanged;
    public bool invincible = false;
    private bool isKnockback = false;
    private float knockbackDuration = 0.0f;
    public float invincibleTimer;

    Vector2 knockback = Vector2.zero;
    Vector2 directionVector;

    List<Transform> monsterCounter = new List<Transform>();
    

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.Find("Sprite").GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
        weapon = GetComponentInChildren<Weapon>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void Start()
    {
        weapons = transform.Find("Weapons");
        EquipWeapon(0);
        weaponAnimator = transform.Find("Weapons").GetComponent<Animator>();

    }
    private void FixedUpdate()
    {
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
            if (knockbackDuration <= 0.0f)
                isKnockback=false;
        }
        if (isKnockback)
            return;
        Move(Vector2.zero);
        
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

        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
                invincible = false;
        }
        

    }


    //Trigger´Â ÇÃ·¹ÀÌ¾î ¹Û¿¡ °ø°Ý¹üÀ§ Å«¿ø
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            monsterCounter.Add(collision.transform);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && isMove == false)
        {
            weapon.AttackTarget(directionVector, shotSpeed, attackSpeed);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (invincible)
            return;
        animator.SetTrigger("IsDamaged");
        if (collision.collider.CompareTag("Enemy"))
        {
            Damaged(5);
            ApplyKnockback(collision.transform);
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


    public override void Move(Vector2 vector)
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = (new Vector2(moveHorizontal, moveVertical).normalized) * moveSpeed;
        rb.velocity = movement;
        isMove = movement.magnitude > 0.5f;
        animator.SetBool("IsMoving", isMove);
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
            Destroy(currentWeapon.gameObject);  // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        }

        // ï¿½ï¿½ï¿½Î¿ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Î½ï¿½ï¿½Ï½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        currentWeapon = Instantiate(weaponPrefabs[weaponIndex], weapons.position, Quaternion.identity);
        currentWeapon.transform.SetParent(weapons);
        weapon = currentWeapon;
    }

    public override void Attack()
    {
        
    }

    public void Damaged(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            //GameOver();
        }
        invincible = true;
        invincibleTimer = 2.0f;
    }

    public void ApplyKnockback(Transform other)
    {
        isKnockback = true;
        knockbackDuration = 0.125f;
        knockback = -(other.position - transform.position).normalized * 4f; 
        rb.velocity = knockback;
    }



    [System.Serializable]
    public class PlayerData
    {
        public string characterName;   // Ä³¸¯ÅÍÀÇ ÀÌ¸§
        public int level = 1;          // Ä³¸¯ÅÍÀÇ ·¹º§, ±âº»°ª 1
        public float maxHealth;        // ÃÖ´ë Ã¼·Â
        public float currentHealth;    // ÇöÀç Ã¼·Â
        public float moveSpeed;        // ÀÌµ¿ ¼Óµµ
        public float attackDamage;     // °ø°Ý·Â
        public float attackSpeed;      // °ø°Ý ¼Óµµ(ÃÊ´ç °ø°Ý È½¼ö)
        public float shotSpeed;
    }

    PlayerData SaveData()
    {
        return new PlayerData
        {
            characterName = this.characterName,
            level = this.level,
            maxHealth = this.maxHealth,
            currentHealth = this.currentHealth,
            moveSpeed = this.moveSpeed,
            attackDamage = this.attackDamage,
            attackSpeed = this.attackSpeed,
            shotSpeed = this.shotSpeed
        };
    }

    void LoadData(PlayerData data )
    {
        this.characterName = data.characterName;
        this.level = data.level;
        this.maxHealth = data.maxHealth;
        this.currentHealth = data.currentHealth;
        this.moveSpeed = data.moveSpeed;
        this.attackDamage = data.attackDamage;
        this.attackSpeed = data.attackSpeed;
        this.shotSpeed = data.shotSpeed;
    }

    void SaveJson()
    {
        PlayerData data = SaveData();
        string json = JsonUtility.ToJson(data, true); // º¸±â ÁÁ°Ô ÀúÀåÇÏ·Á¸é true
        string path = Application.persistentDataPath + "/player.json";

        File.WriteAllText(path, json);
        Debug.Log("ÀúÀå ¿Ï·á: " + path);
    }

    void LoadFromFile()
    {
        string path = Application.persistentDataPath + "/player.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            LoadData(data);
            Debug.Log("ºÒ·¯¿À±â ¿Ï·á");
        }
        else
        {
            Debug.LogWarning("ÀúÀå ÆÄÀÏÀÌ ¾ø½À´Ï´Ù: " + path);
        }
    }
}
