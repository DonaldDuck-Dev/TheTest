using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    protected KeyCode shootButton = KeyCode.Mouse0;
    public KeyCode reloadButton = KeyCode.R;

    //[Serializefield] gör så att du kan se privata variabler i Unity inspektorn. 
    AudioSource audioSource;
    public AudioClip gunShotNoise;
    public AudioClip outOfAmmoNoise;
    public AudioClip reloadNoise;

    public Transform shootPoint;
    public int ammo = 2, ammoCap = 2, ammoAv = 10;
    [SerializeField] float damage = 10, range = 20;
    protected int knockBack = 200;

    float reloadTime = 1;
    public Material material;
    Renderer render;

    [SerializeField] float coolDownTime = 0.5f;
    
    public bool isOnCoolDown, isReloading;

    RaycastHit hit;
    
    private void Start()
    {
        render = gameObject.GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    
    private void Update()
    {
        Debug.DrawRay(shootPoint.position, shootPoint.forward * range, Color.green);

        interact();
    }

    public virtual void interact()
    {
        if (Input.GetKeyDown(shootButton))
        {
            shoot();
        }

        if (Input.GetKeyDown(reloadButton))
        {
            StartCoroutine("reload");
        }
    }

    /*Den här metoden skjutar ut en linje, och kollar om den träffar något med tag "Enemy".
    * 'Range' float är längden på linjen.
    * Om ditt vapen har tillräckligt 'ammo', så körs 'takeDamage' metoden i 'Enemy' scripten.
    * Då körs coolDown metoden och väntar 'coolDownTime' sekunder. */

    public virtual void shoot()
    {
        if (!isOnCoolDown && ammo > 0 && !isReloading)
        {

            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, range) && hit.collider.CompareTag("Enemy"))
            {
                if (ammo > 0)
                {
                    hit.collider.GetComponent<Enemy>().takeDamage(damage);
                    hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * knockBack, ForceMode.Impulse);
                }
            }

            StartCoroutine(coolDown(coolDownTime));

            ammo--;
            audioSource.PlayOneShot(gunShotNoise);
            Debug.Log("Ammo left: " + ammo);


        }
        else if (ammo == 0 && !isReloading)
        {

            Debug.Log("Out of ammo!");
            audioSource.PlayOneShot(outOfAmmoNoise);
        }
    }


    //cooldown timer
    IEnumerator coolDown(float seconds)
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(seconds);
        isOnCoolDown = false;
    }

    public IEnumerator reload()
    {

        if (ammo != ammoCap && !isReloading && ammoAv > 0)
        {
            isReloading = true;
            audioSource.PlayOneShot(reloadNoise);
            yield return new WaitForSeconds(reloadTime);
            isReloading = false;

            int ammoDif = ammoCap - ammo;

            if (ammoAv > ammoDif)
            {
                ammo += ammoDif;
                ammoAv -= ammoDif;
            }
            else
            {
                ammo += ammoAv;
                ammoAv = 0;
            }
        }
        else if(ammoAv < 0)
        {
            Debug.Log("Out of ammo!");
        }
    }
}
