using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] Enemy;
    public GameObject Ammunition;
    public Text roundText;

    int enemyCount;
    int round = 1;
    [SerializeField] int spawnCount = 2;

    Vector3 spawnPos;
    public LayerMask isPlayer, isEnemy;
    [SerializeField]float disFromPlayer = 15f;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemy(round);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount == 0)
        {
            round += spawnCount;
            spawnEnemy(round);
            spawnAmmo(1);   
        }
        roundText.text = "Enemies remaining: " + enemyCount.ToString();
    }

    bool foundSpawnPos;

    void spawnEnemy(int enemyCount)
    {
        int i = 0;
        while (i < enemyCount)
        {
            foundSpawnPos = false;
            findSpawnPos();

            while (!foundSpawnPos)
            {
                if (Physics.CheckSphere(spawnPos, 10, isPlayer) && Physics.CheckSphere(spawnPos, 5, isEnemy))
                {
                    findSpawnPos();
                }
                else
                    foundSpawnPos = true;
            }

            if (foundSpawnPos)
            {
                int enemyIndex = Random.Range(0, Enemy.Length);

                Instantiate(Enemy[enemyIndex], spawnPos, Enemy[enemyIndex].transform.rotation);
            }
            else if (!foundSpawnPos)
            {
                Debug.LogError("Couldn't spawn enemy(" + i + ")");
            }

            i++;
        }

    }
    void spawnAmmo(int amount)
    {
        findSpawnPos();
        Instantiate(Ammunition, spawnPos, Ammunition.transform.rotation);
    }
    
    void findSpawnPos()
    {
        //If random number == 0, -30. else 30;
        //float randomPosX = Random.Range(0, 2) == 0 ? -30 : 30;
        //float randomPosZ = Random.Range(0, 2) == 0 ? -12 : 12;


        float randomPosX = Random.Range(-30, 30);
        float randomPosZ = Random.Range(-12, 12);

        spawnPos = new Vector3(randomPosX, 1, randomPosZ);

    }
    
}
