using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private Transform toAttack;
    private Animator anim;
    private NavMeshAgent agent;
    private Collider col;
    public int health = 1;
    
    // Start is called before the first frame update
    private Coroutine atk = null;
    void Start()
    {
        col = GetComponent<Collider>();
        toAttack = GameObject.Find("Defence point").transform;
        agent = GetComponent<NavMeshAgent>();
        if (GameManager.map == 1)
            agent.destination = new UnityEngine.Vector3(Random.Range(-25,25), toAttack.position.y, toAttack.position.z);
        else if (GameManager.map == 2)
            agent.destination = new UnityEngine.Vector3(Random.Range(5,7),toAttack.position.y, Random.Range(-9, 0));
        atk = StartCoroutine(attack());
        anim = GetComponent<Animator>();
        StartCoroutine(checkDeath());

        GameManager.ennemies += 1;
    }

    // Update is called once per frame
    void Update()
    {
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
            if (agent.remainingDistance <= 1)
            {
                GameManager.defHp -= 1;
                anim.SetTrigger("attack");
            }
        }
    }
}
