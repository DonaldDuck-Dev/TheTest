using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public Text[] inventoryText = new Text[2];

    //Add a method that increases/decreases the index through scrollwheel.

    [SerializeField] GameObject[] inventory = new GameObject[4];
    int selectedGun = 0;
    [SerializeField] string weaponSelected;

    Gun gun = new Gun();
    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        weaponSelect();

        inventoryText[1].text = inventory[selectedGun].name;
        if (inventory[selectedGun].GetComponent<Gun>() != null)
        {
            gun = inventory[selectedGun].GetComponent<Gun>();
            inventoryText[0].text = inventory[selectedGun].GetComponent<Gun>().ammo.ToString() + "/" + inventory[selectedGun].GetComponent<Gun>().ammoAv.ToString();

        }
        else
        {
            gun = null;
            inventoryText[0].text = "";
            inventoryText[1].text = "";
        }

        //Texten som säger vilken vapen du håller och hur mycket ammo du har

    }


    //Man kan byta mellan vapen 
    private void weaponSelect()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel > 0)
        {
            //Om man ändrar vapen när man laddar om vapnet fastnar den på isReloading (Då funkar inte den)
            if (gun != null)
            {
                inventory[selectedGun].GetComponent<Gun>().isReloading = false;
                inventory[selectedGun].GetComponent<Gun>().isOnCoolDown = false;
            }

            selectedGun++;

            if (selectedGun < inventory.Length)
            {
                inventory[selectedGun-1].SetActive(false);
                inventory[selectedGun].SetActive(true);
            }
            else
            {

                inventory[selectedGun-1].SetActive(false);
                selectedGun = 0;
                inventory[selectedGun].SetActive(true);
            }
            scrollWheel = 0;

            Debug.Log("pressed E    " + selectedGun);

            weaponSelected = inventory[selectedGun].name;
        }
        else if(scrollWheel < 0)
        {
            if (gun != null)
            {
                inventory[selectedGun].GetComponent<Gun>().isReloading = false;
                inventory[selectedGun].GetComponent<Gun>().isOnCoolDown = false;
            }
            selectedGun--;
            if (selectedGun >= 0)
            {
                inventory[selectedGun + 1].SetActive(false);
                inventory[selectedGun].SetActive(true);
            }
            else
            {

                inventory[0].SetActive(false);
                selectedGun = inventory.Length - 1;
                inventory[selectedGun].SetActive(true);
            }
            scrollWheel = 0;
        }
        
    }
    
}
