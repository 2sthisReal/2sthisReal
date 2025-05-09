using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bow : Weapon
{

    GameObject arrowPrefabs;
    // Start is called before the first frame update
    public override void AttackMotion()
    {
        base.AttackMotion();
        GameObject arrow = Instantiate(arrowPrefabs, weapontransform.position , Quaternion.Euler(0,0,0) ,weapontransform.parent);
        arrow.GetComponent<ProjectileController>().Init(directionVector,shotSpeed);
    }

    
}
