using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private static AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Play()
    {
        particleSystem.Play();
        source.Play();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
            other.gameObject.GetComponent<Enemy>().health -= GameManager.slashDmg;
        else if (other.gameObject.GetComponent<Soldier>() != null)
            other.gameObject.GetComponent<Soldier>().health -= GameManager.slashDmg;
    }
}