using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/* Fait par Clément Pera
 * Fait le 07 Mai 2023
 * 
 * Ce script gère les monstres de type Araignée
 */
public class Araigne : MonoBehaviour
{
    private GameManager gameManager; //Ceci importe le script GameManager
    private Transform toAttack; //Ceci est la position de l'objet à attaquer
    private Animator anim; //Ceci est l'animator de l'objet
    private NavMeshAgent agent; //Ceci est l'agent de navigation de l'objet
    private Collider col; //Ceci est le collider de l'objet
    public int health = 1; //Ceci est la vie de l'objet
    private Coroutine atk = null; //Ceci est la coroutine d'attaque de l'objet
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        
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

        gameManager.ennemies += 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Cette coroutine vérifie si la vie de l'objet est à 0, si c'est le cas, il meurt
    IEnumerator checkDeath()
    {
        bool isDead = false;
        while (!isDead)
        {
            if (health <= 0)
            {
                gameManager.ennemies -= 1;
                
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
    
    //Cette coroutine tue l'objet après son animation
    IEnumerator die()
    {
        GameManager.maxEnnemies += 1;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    //Cette coroutine gère l'attaque de l'objet en direction de l'objet à attaquer
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
