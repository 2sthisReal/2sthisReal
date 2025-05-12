using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    Player player;
    public Weapon weapon;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
        if (player == null)
            Debug.Log("error");
        

    }
    // Start is called before the first frame update
    private void Start()
    {
        weapon = player.transform.Find("Weapons").GetComponentInChildren<Weapon>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            player.monsterCounter.Add(collision.transform);
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && player.isMove == false)
        {
            if (weapon == null)
            {
                Debug.LogError("weapon not found");
                return;
            }
            weapon.AttackTarget(player.directionVector, player.shotSpeed, player.attackSpeed, player.multipleShots);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.monsterCounter.Remove(collision.transform);
    }
}
