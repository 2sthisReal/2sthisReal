using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    protected Transform weapontransform;
    protected Animator animator;

    protected Vector2 directionVector;


    [Header("¹«±â½ºÅÝ")]
    public float attack;
    public int numberOfShot;

    protected float attackCooldown = 0f;
    private void Awake()
    {
        weapontransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Rotate(directionVector);
    }

    public void GetVector(Vector2 direction)
    {
        directionVector = direction;
    }
    public void Rotate(Vector2 direction)
    {

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weapontransform.localRotation = Quaternion.Euler(0f, 0f, rotZ);
        if (rotZ < 0f) rotZ += 360f;

    }
    public virtual void AttackMotion()
    {
        animator.SetBool("IsShoot", true);

    }

    public void WeaponReady()
    {
        animator.SetBool("IsRanged", true);
    }

    public void WeaponWait()
    {
        animator.SetBool("IsRanged", false);
    }
    public virtual void AttackTarget(Vector2 vector, float ShotSpeed, float Rate, bool multipleShot)
    {

    }
}

