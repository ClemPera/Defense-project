using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

/* Fait par Clément Pera
 * Fait le 07 Mai 2023
 * 
 * Ce script les ennemies de type soldats
 */
public class Soldier : MonoBehaviour
{
    private GameManager gameManager; //Le game manager
    private Transform toAttack; //Le transform point à attaquer
    public GameObject projectilePrefab; //Le prefab du projectile
    private NavMeshAgent agent; //L'agent NavMesh
    public int health = 5; //La vie du soldat
    private Animator anim; //L'animator de l'objet
    private Collider col; //Le collider de l'objet

    private Coroutine dest = null; //La coroutine de destination
    private Coroutine atk = null; //La coroutine d'attaque
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>(); 
        agent = GetComponent<NavMeshAgent>();
        toAttack = GameObject.Find("Player").transform;
        atk = StartCoroutine(attack());
        StartCoroutine(checkDeath());
        dest = StartCoroutine(destination());
        
        gameManager.ennemies += 1;
    }

    // Update is called once per frame
    void Update()
    {
        toAttack = GameObject.Find("Player").transform;
    }
    
    //Cette coroutine assigne la destination à l'emplacememnt du joueur
    IEnumerator destination()
    {
        while (true)
        {
            agent.destination = new Vector3(toAttack.position.x,toAttack.position.y, toAttack.position.z);
            yield return null;
        }

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

    //Cette coroutine fait mourir l'objet
    IEnumerator die()
    {
        GameManager.maxEnnemies += 1;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    
    //Cette coroutine gère l'instantiation des projectiles
    IEnumerator attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Instantiate(projectilePrefab, transform.position, transform.rotation);
        }
    }
}
