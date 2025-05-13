using UnityEngine;

public class OrbitingSword : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float radius = 1.5f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float angleOffset;
    [SerializeField] float damageScale = 1.5f;
    Player player;

    float currentAngle;

    public void Init(Transform center, Player player, float damageScale)
    {
        this.center = center;
        this.player = player;
        this.damageScale = damageScale;
    }

    void Update()
    {
        if (center == null) return;

        // 각도 증가
        currentAngle += rotateSpeed * Time.deltaTime;
        float angle = currentAngle + angleOffset;

        // 위치 계산
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        transform.position = center.position + new Vector3(x, y, 0);

        // 손잡이가 캐릭터를 바라보도록 회전
        Vector3 direction = center.position - transform.position;
        float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0, angleZ + 90f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<BaseCharacter>().TakeDamage(damageScale * player.attackDamage);
        }
    }
}
