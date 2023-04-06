using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;
    private Transform toAttack;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        toAttack = GameObject.Find("Defence point").transform;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = new UnityEngine.Vector3(UnityEngine.Random.Range(-25,25), toAttack.position.y, toAttack.position.z);
        StartCoroutine(attack());
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (transform.position.z < 0.0f)
            {
                gameManager.defHp -= 1;
                anim.SetTrigger("attack");
            }
        }
    }
}
