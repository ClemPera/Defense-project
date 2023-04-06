using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Soldier : MonoBehaviour
{
    public float speed;
    private GameObject player;
    private Transform toAttack;
    public GameObject projectilePrefab;
    private NavMeshAgent agent;
    public int health = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        toAttack = GameObject.Find("Player").transform;
        StartCoroutine(attack());
    }

    // Update is called once per frame
    void Update()
    {
        toAttack = GameObject.Find("Player").transform;
        agent.destination = new UnityEngine.Vector3(toAttack.position.x,toAttack.position.y, toAttack.position.z);
        
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Instantiate(projectilePrefab, transform.position, transform.rotation);
        }
    }
}
