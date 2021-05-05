using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    float range = 2;
    float damage = 5;

    public Transform attackPoint;
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(attackPoint.position, attackPoint.forward * range, Color.green);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, range))
            {
                hit.collider.GetComponent<Enemy>().takeDamage(damage);
            }
        }
    }
}
