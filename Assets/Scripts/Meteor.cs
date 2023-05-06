using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private Dictionary<Collider, Coroutine> dmgCoroutines = new Dictionary<Collider, Coroutine>();
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other) {
        // Stocker la coroutine dans un élément de la Dictionary correspondant à l'objet qui a déclenché le OnTriggerEnter
        if (other.gameObject.GetComponent<Enemy>() != null || other.gameObject.GetComponent<Soldier>() != null)
        {
            Coroutine coroutine = StartCoroutine(Damage(other));
            dmgCoroutines.Add(other, coroutine);
        }
    }

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

    IEnumerator Damage(Collider other)
    {
        while (true)
        {
            try
            {
                if (other.gameObject.GetComponent<Enemy>() != null)
                    other.gameObject.GetComponent<Enemy>().health -= GameManager.slashDmg;
                else if (other.gameObject.GetComponent<Soldier>() != null)
                    other.gameObject.GetComponent<Soldier>().health -= GameManager.slashDmg;
            }
            catch (MissingReferenceException e)
            {
                break;
            }

            yield return new WaitForSeconds(1);
        }
    }
}