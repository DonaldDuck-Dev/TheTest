using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 2;
    public float speed = 5;
    public float sprintSpeed = 10;
    public float sprintDuration;
    public bool isSprinting;
    public bool onGround;

    [SerializeField] float health = 10;
    public Text healthText;
    [SerializeField] float invulnerability = 0.5f;
    bool isInvulnerable;

    [SerializeField] Rigidbody playerRB;

    public Camera playerCamera;

    public AudioClip hurtNoise;
    public AudioClip[] footSteps;
    AudioSource playerAudio;
    bool hasPlayed;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerRB = GetComponent<Rigidbody>();
    }

    float _speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        //Look at mouse II.o
        Ray cameraRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            pointToLook.y = transform.position.y;
            transform.LookAt(pointToLook);
        }

        //Ändrar din hasighet om man trycker på 'shift'
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            _speed = sprintSpeed;
        }
        else
        {
            isSprinting = false;
            _speed = speed;
        }

        Vector3 direction = new Vector3(verticalInput, 0, horizontalInput).normalized;

        playerRB.velocity = transform.forward * verticalInput * _speed * 100 * Time.deltaTime;

        //Kontrollerar fotsteg ljud
        if ((Input.GetKey("s") || Input.GetKey("w")) && !hasPlayed && onGround)
        {
            if (isSprinting){ StartCoroutine(playFootSteps(3)); } else { StartCoroutine(playFootSteps(2)); }
        }

        if (health <=0)
        {
            Destroy(gameObject);
            Debug.Log("Game over");
        }

        //Visar din din 'health' i spelet.
        healthText.text = "Health: " + health;
    }
    
    //Fotsteg ljud kontroll, som väntar tills föra ljudklippet har spelats (speed).
    public IEnumerator playFootSteps(float speed)
    {
        hasPlayed = true;
        int footStepType = Random.Range(0, footSteps.Length);
        playerAudio.PlayOneShot(footSteps[footStepType], 0.1f);
        yield return new WaitForSeconds(footSteps[footStepType].length/speed);
        hasPlayed = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.layer == 9)
        {
            onGround = true;
        }
        else
            onGround = false;
    }
    public void takeDamage(float damage)
    {
        if (!isInvulnerable)
        {
            playerAudio.PlayOneShot(hurtNoise);
            health -= damage;
            Debug.Log("health is at " + health);
            StartCoroutine(inulnerable());
        }
    }
    

    void constraints()
    {
        if (transform.position.x < -34)
        {
            transform.position = new Vector3(-34, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 34)
        {
            transform.position = new Vector3(34, transform.position.y, transform.position.z);
        }
        else if (transform.position.z < -12)
            transform.position = new Vector3(transform.position.x, transform.position.y, -12);
        else if (transform.position.z > 12)
            transform.position = new Vector3(transform.position.x, transform.position.y, 12);
    }

    IEnumerator inulnerable()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerability);
        isInvulnerable = false;
    }
}
