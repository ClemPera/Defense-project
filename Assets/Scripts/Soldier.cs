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
    private Animator anim;
    private Collider col;
    
    // Start is called before the first frame update

    private Coroutine dest = null;
    private Coroutine atk = null;
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>(); 
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        toAttack = GameObject.Find("Player").transform;
        atk = StartCoroutine(attack());
        StartCoroutine(checkDeath());
        dest = StartCoroutine(destination());
        
        GameManager.ennemies += 1;
    }

    // Update is called once per frame
    void Update()
    {
        toAttack = GameObject.Find("Player").transform;
    }
    
    IEnumerator destination()
    {
        while (true)
        {
            agent.destination = new Vector3(toAttack.position.x,toAttack.position.y, toAttack.position.z);
            yield return null;
        }

    }

    IEnumerator checkDeath()
    {
        bool isDead = false;
        while (!isDead)
        {
            if (health <= 0)
            {
                GameManager.ennemies -= 1;
                StopCoroutine(atk);
                StopCoroutine(dest);
                anim.SetTrigger("die");
                col.enabled = false;
                agent.enabled = false;
                StartCoroutine(die());
                isDead = true;
            }

            yield return null;
        }
    }

    IEnumerator die()
    {
        GameManager.maxEnnemies += 1;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
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
