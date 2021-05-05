using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    public int ammoAmount = 10;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponentInChildren<Gun>() != null)
            {
                Destroy(gameObject);
                other.gameObject.GetComponentInChildren<Gun>().ammoAv += ammoAmount;
            }
        }
    }
}
