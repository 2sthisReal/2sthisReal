using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Animator spriteAnimator;
    Animator weaponAnimator;

    public bool inRanged;
    int enemyCount = 0; //���� ���� ���� ���� ����

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            enemyCount++;
            if(enemyCount >= 1) //���� 1�� �̻��� ��
            {
                inRanged = true;
            }
            else
            {
                inRanged = false;
            }
            weaponAnimator.SetBool("IsRanged", inRanged);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            enemyCount--;
            if(enemyCount <= 0) //���� 0�� ������ ��
            {
                inRanged = false;
            }
            else
            {
                inRanged = true;
            }
            weaponAnimator.SetBool("IsRanged", inRanged);
        }
    }
}
