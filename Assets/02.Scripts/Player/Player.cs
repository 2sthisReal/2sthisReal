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

    public bool isMove;
    public bool inRanged;
    public bool invincible = false;
    private bool isKnockback = false;
    private float knockbackDuration = 0.0f;
    public float invincibleTimer;

    Vector2 knockback = Vector2.zero;
    public Vector2 directionVector;

    public List<Transform> monsterCounter = new List<Transform>();
    

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.Find("Sprite").GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
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
        if (!isMove)
        { 
            rb.velocity = Vector2.right * 0.000001f;
            rb.velocity = Vector2.left * 0.000001f;
        }


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


    //Trigger는 플레이어 밖에 공격범위 큰원
      
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌");
        if (invincible)
            return;
        animator.SetTrigger("IsDamaged");
        if (collision.collider.CompareTag("Enemy"))
        {
            Damaged(5);
            ApplyKnockback(collision.transform);
        }
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
            Destroy(currentWeapon.gameObject);  // 현재 장착된 무기 제거
        }

        // 새로운 무기 인스턴스를 장착
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
        public string characterName;   // 캐릭터의 이름
        public int level = 1;          // 캐릭터의 레벨, 기본값 1
        public float maxHealth;        // 최대 체력
        public float currentHealth;    // 현재 체력
        public float moveSpeed;        // 이동 속도
        public float attackDamage;     // 공격력
        public float attackSpeed;      // 공격 속도(초당 공격 횟수)
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
        string json = JsonUtility.ToJson(data, true); // 보기 좋게 저장하려면 true
        string path = Application.persistentDataPath + "/player.json";

        File.WriteAllText(path, json);
        Debug.Log("저장 완료: " + path);
    }

    void LoadFromFile()
    {
        string path = Application.persistentDataPath + "/player.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            LoadData(data);
            Debug.Log("불러오기 완료");
        }
        else
        {
            Debug.LogWarning("저장 파일이 없습니다: " + path);
        }
    }
}
