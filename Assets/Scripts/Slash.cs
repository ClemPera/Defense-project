using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fait par Clément Pera
 * Fait le 07 Mai 2023
 * 
 * Ce script gère les attaques de type Slash du joueur 
 */
public class Slash : MonoBehaviour
{
    private ParticleSystem particle;
    private static AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Joue l'animation et le son du slash
    public void Play()
    {
        particle.Play();
        source.Play();
    }
    
    //Si un ennemie est touché, il perd de la vie
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Araigne>() != null)
            other.gameObject.GetComponent<Araigne>().health -= GameManager.slashDmg;
        else if (other.gameObject.GetComponent<Soldier>() != null)
            other.gameObject.GetComponent<Soldier>().health -= GameManager.slashDmg;
    }
}