using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Animator spriteAnimator;
    Animator weaponAnimator;

    public bool inRanged;
    //�߻�Ŭ�������� ���� ����
    [SerializeField]float tempspeed = 3;   //�÷��̾��� �ӵ�




    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon_bow").GetComponent<Animator>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = (new Vector2(moveHorizontal, moveVertical).normalized) * tempspeed;
        rigidbody.velocity = movement;
        spriteAnimator.SetBool("IsMoving", movement.magnitude > 0.5f);    
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            inRanged = true;
            weaponAnimator.SetBool("IsRanged", inRanged);
        }
        else
            inRanged = false;
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
