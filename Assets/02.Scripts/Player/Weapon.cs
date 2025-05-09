using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Coroutine attackCoroutine;

    protected Transform weapontransform;
    protected Animator animator;

    protected Vector2 directionVector;
    

    [Header("¹«±â½ºÅÝ")]
    public float attack;
    public float rate;
    public float shotSpeed;
    public int numberOfShot;

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
        animator.SetBool("IsRanged" , true);
    }

    public void WeaponWait()
    {
        animator.SetBool("IsRanged", false);
    }

    public void StartAttack()
    {
        if (attackCoroutine == null)
            attackCoroutine = StartCoroutine(AttackRoutine(rate));
    }

    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }


    IEnumerator AttackRoutine(float attackRate)
    {
        AttackMotion();
        yield return new WaitForSeconds(1f / attackRate);
    }

}
