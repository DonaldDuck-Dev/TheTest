using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;

    [Range(10, 30)]public float yOffset = 24;

    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        offset.y = yOffset;
        transform.position = player.transform.position + offset;
    }
}
