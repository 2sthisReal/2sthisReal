using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;



public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform pivot;
    Rigidbody2D rb;
    Player player;
    public AudioClip arrowSound;
    public AudioClip arrowHit;
    float arrowSustain = 3f;


    private double projectileDamage = 0;

    // ArrowSkills
    [SerializeField] bool canPierce = false;
    [SerializeField] bool canWallReflect = false;
    [SerializeField] bool canMonsterBounce = false;
    int bouceCount = 3;

    // 이미 맞은 몬스터 관리
    List<Monster> hitMonsters = new List<Monster>();



    private void Awake()
    {
        pivot = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        arrowSustain -= Time.deltaTime;
        if (arrowSustain < 0)
            Destroy(this.gameObject);
    }
    public void Init(Vector2 direction, float shotspeed, List<ArrowSkillType> arrowSkills)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.velocity = direction * shotspeed;
        SoundManager.PlayClip(arrowSound);
        foreach (var skill in arrowSkills)
        {
            switch (skill)
            {
                case ArrowSkillType.Pierce:
                    canPierce = true;
                    break;

                case ArrowSkillType.WallReflect:
                    canWallReflect = true;
                    break;

                case ArrowSkillType.MonsterBounce:
                    canMonsterBounce = true;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            SoundManager.PlayClip(arrowHit);

            if (monster != null)
            {
                CritCalculator();
                monster.TakeDamage((float)projectileDamage);
                projectileDamage = player.attackDamage;
            }

            // 몬스터 반사가 있다면 가까운 몬스터로 이동
            if (canMonsterBounce)
            {
                if (bouceCount <= 0)
                    Destroy(gameObject);

                // 맞은 몬스터 추가
                hitMonsters.Add(monster);

                BounceToNearMonster();

                bouceCount--;
                return;
            }

            // 관통이 없다면 몬스터와 충돌 시 파괴
            if (!canPierce)
            {
                Destroy(this.gameObject);
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            SoundManager.PlayClip(arrowHit);
            // 벽 반사가 존재한다면 벽 반사
            if (canWallReflect)
            {
                if (bouceCount <= 0)
                    Destroy(gameObject);

                ReflectWall(collision.ClosestPoint(transform.position));

                bouceCount--;
                return;
            }

            else
                Destroy(this.gameObject);
        }
    }

    private void CritCalculator()
    {
        projectileDamage = player.attackDamage;
        float rand = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(rand);
        if (rand < player.critRate)
            projectileDamage = player.attackDamage * 1.5;
    }
    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    // 벽과 충돌하면 반사
    private void ReflectWall(Vector2 hitPoint)
    {
        Vector2 normal = (transform.position - (Vector3)hitPoint).normalized;
        Vector2 reflect = Vector2.Reflect(rb.velocity, normal);

        rb.velocity = reflect;

        float angle = Mathf.Atan2(reflect.y, reflect.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void BounceToNearMonster()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        Transform nearestMonster = null;
        float minDistant = float.MaxValue;

        foreach (var monster in monsters)
        {
            // 피격되지 않은 몬스터중에 가장 가까운 몬스터에게 이동
            float dist = Vector2.Distance(transform.position, monster.transform.position);

            if (dist < minDistant && !hitMonsters.Contains(monster))
            {
                minDistant = dist;
                nearestMonster = monster.transform;
            }
        }

        if (nearestMonster != null)
        {
            // 방향 조정
            Vector2 direction = (nearestMonster.position - transform.position).normalized;
            rb.velocity = direction * rb.velocity.magnitude;

            // 해당 방향으로 회전
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        else
            Destroy(this.gameObject);
    }
}
