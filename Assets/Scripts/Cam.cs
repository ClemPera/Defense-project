using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset = new Vector3(0, 20, -5);

    private int distance = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position - player.transform.forward * distance + offset;
    }
}
