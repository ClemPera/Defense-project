using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/* Fait par Clément Pera
 * Fait le 07 Mai 2023
 * 
 * Ce script gère attaque de type météore du joueur
 */
public class Meteor : MonoBehaviour
{
    private Dictionary<Collider, Coroutine> dmgCoroutines = new Dictionary<Collider, Coroutine>(); //Dictionaire qui stocke les coroutines de dégats 
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Détruit le météore après 4 secondes
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
    
    
    //Si un ennemie est touché, lance la coroutine de dégats
    private void OnTriggerEnter(Collider other) {
        // Stocker la coroutine dans un élément de la Dictionary correspondant à l'objet qui a déclenché le OnTriggerEnter
        if (other.gameObject.GetComponent<Araigne>() != null || other.gameObject.GetComponent<Soldier>() != null)
        {
            Coroutine coroutine = StartCoroutine(Damage(other));
            dmgCoroutines.Add(other, coroutine); //Ajoute la coroutine au dictionnaire, cela permet de gérer plusieurs ennemies en même temps
        }
    }

    //Si un ennemie sort du trigger, arrête la coroutine de dégats de l'ennemie
    private void OnTriggerExit(Collider other) {
        // Vérifier si l'objet qui a déclenché le OnTriggerExit a une coroutine stockée
        if (dmgCoroutines.ContainsKey(other))
        {
            // Arrêter la coroutine correspondante à l'objet qui a déclenché le OnTriggerExit
            Coroutine coroutine = dmgCoroutines[other];
            StopCoroutine(coroutine);

            // Retirer l'élément de la Dictionary correspondant à l'objet qui a déclenché le OnTriggerExit
            dmgCoroutines.Remove(other);
        }
    }

    //Coroutine qui fait perdre de la vie à l'ennemie
    IEnumerator Damage(Collider other)
    {
        while (true)
        {
            try
            {
                if (other.gameObject.GetComponent<Araigne>() != null)
                    other.gameObject.GetComponent<Araigne>().health -= GameManager.slashDmg;
                else if (other.gameObject.GetComponent<Soldier>() != null)
                    other.gameObject.GetComponent<Soldier>().health -= GameManager.slashDmg;
            }
            catch (MissingReferenceException)
            {
                break;
            }

            yield return new WaitForSeconds(1);
        }
    }
}