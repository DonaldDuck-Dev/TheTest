using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameObject player;
    [SerializeField] float health = 10.0f;
    [SerializeField] float enemySpeed = 5.0f;
    [SerializeField] float damage = 5.0f;

    AudioSource enemyAudio;
    public AudioClip[] hurt;
    public AudioClip dieNoise;

    Rigidbody enemyRb;

    NavMeshAgent agent;

    float rotationSpeed = 10.0f;

    //walking
    bool walkPosFound, reachedWalkPos;
    Vector3 walkPos;
    float idleTime = 2;

    //attacking
    public bool seePlayer;
    float sightRange = 20.0f, walkRange = 14.0f;

    [SerializeField]LayerMask isPlayer = 1<<8, ground = 1<<9, obsticle = 1<<10;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        enemyAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Kollar bara på spelaren på x axeln och z axeln. 
        Vector3 lookAtPoint = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        agent.speed = enemySpeed;
        if (!seePlayer)
            patrol();
        else
        {
            transform.LookAt(lookAtPoint);
            //lookAtPLayer();
            agent.SetDestination(player.transform.position);
        }
        seePlayer = Physics.CheckSphere(transform.position, sightRange, isPlayer);

        if(health <= 0)
        {
            die();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Debug.DrawLine(transform.position, walkPos);
    }
    
    private void patrol()
    {
        if (!walkPosFound)
            findWalkPos();
        else
        {
            agent.SetDestination(walkPos);
            //Fiender tittar på walkPos men vrider inte upp och ner på den
            transform.LookAt(new Vector3(walkPos.x, transform.position.y, walkPos.z));
        }

        Vector3 distanceToWalk = transform.position - walkPos;

        if(distanceToWalk.magnitude < 4f && !reachedWalkPos)
        {
            reachedWalkPos = true;
            StartCoroutine("idle");
        }
    }  

    //Hittar en position inom en cirkel, och kollar om den ligger på navmesh
    private void findWalkPos()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 20;
        randomDirection += transform.position;
        NavMeshHit hit;
        //Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, 20, 1))
        {
            walkPos = hit.position;
            walkPosFound = true;
        }
    }
    IEnumerator idle()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(idleTime);
        agent.isStopped = false;
        walkPosFound = false;
        reachedWalkPos = false;
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        enemyAudio.PlayOneShot(hurt[Random.Range(1, hurt.Length)]);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().takeDamage(damage);
        }
    }
    void die()
    {
        Destroy(gameObject);
    }

/*
    void lookAtPLayer()
    {
        //Gets a vector between the player.
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(direction);
        
        
        //Rotates the enemy towards the player.
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, rotationSpeed * Time.deltaTime);
    }

    void moveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
    }
*///Behövs inte <--
}
