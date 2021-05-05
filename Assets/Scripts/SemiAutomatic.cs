using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutomatic : Gun
{
    
    public override void interact()
    {
        if (Input.GetKey(shootButton) && ammo != 0)
        {
            shoot();
        }
        else if(Input.GetKeyDown(shootButton) && ammo == 0)
            shoot();
        
        if (Input.GetKeyDown(reloadButton))
        {
            StartCoroutine("reload");
        }
    }

    public override void shoot()
    {
        knockBack = 2;
        base.shoot();
    }
}
