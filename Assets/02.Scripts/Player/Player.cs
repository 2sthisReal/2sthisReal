using SWScene;
using System.Collections;
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
    public bool multipleShots = false;
    private float knockbackDuration = 0.0f;
    public float invincibleTimer;
    public float critRate = 0.0f;

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
        LoadFromFile();
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
        //trigger�� �������� �۵��Ǳ⶧���� �� �ļ�
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


    //Trigger�� �÷��̾� �ۿ� ���ݹ��� ū��
      
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (invincible)
            return;
        Monster monster = collision.gameObject.GetComponent<Monster>();
        
        if (collision.collider.CompareTag("Enemy"))
        {
            animator.SetTrigger("IsDamaged");
            TakeDamage(monster.attackDamage);
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
            Destroy(currentWeapon.gameObject);  // ���� ������ ���� ����
        }

        // ���ο� ���� �ν��Ͻ��� ����
        currentWeapon = Instantiate(weaponPrefabs[weaponIndex]);
        currentWeapon.transform.SetParent(weapons);
        currentWeapon.transform.localPosition = Vector3.zero;
        weapon = currentWeapon;
    }

    public override void Attack()
    {
        
    }

    public override void TakeDamage(float damage)
    {
        if (invincible)
            return;

        currentHealth -= damage;
        if (currentHealth < 0)
        {
            //GameOver();
            //SaveJson();
            GameManager.Instance.ChangeState(GameState.GameOver);
            Debug.Log("Game Over");
        }
        invincible = true;
        invincibleTimer = 2.0f; //2�ʹ���
        StartCoroutine(BlinkAlpha(2.0f, 0.1f));
    }

    public void ApplyKnockback(Transform other)
    {
        isKnockback = true;
        knockbackDuration = 0.125f;
        knockback = -(other.position - transform.position).normalized * 4f; 
        rb.velocity = knockback;
    }

    IEnumerator BlinkAlpha(float duration, float frequency)
    {
        float timer = 0f;
        Color originalColor = spriteRenderer.color;

        while (timer < duration)
        {
            Color c = spriteRenderer.color;
            c.a = (c.a == 1f) ? 0.2f : 1f;
            spriteRenderer.color = c;

            yield return new WaitForSeconds(frequency);
            timer += frequency;
        }

        spriteRenderer.color = originalColor; // ������� ����
    }
    public void ToggleMultiShot()
    {
        multipleShots = !multipleShots;
    }








    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>



    [System.Serializable]
    public class PlayerData
    {
        public string characterName;   // ĳ������ �̸�
        public int level = 1;          // ĳ������ ����, �⺻�� 1
        public float maxHealth;        // �ִ� ü��
        public float currentHealth;    // ���� ü��
        public float moveSpeed;        // �̵� �ӵ�
        public float attackDamage;     // ���ݷ�
        public float attackSpeed;      // ���� �ӵ�(�ʴ� ���� Ƚ��)
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
        string json = JsonUtility.ToJson(data, true); // ���� ���� �����Ϸ��� true
        string path = Application.persistentDataPath + "/player.json";

        File.WriteAllText(path, json);
        Debug.Log("���� �Ϸ�: " + path);
    }

    void LoadFromFile()
    {
        string path = Application.persistentDataPath + "/player.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            LoadData(data);
            Debug.Log("�ҷ����� �Ϸ�");
        }
        else
        {
            Debug.LogWarning("���� ������ �����ϴ�: " + path);
        }
    }
}
