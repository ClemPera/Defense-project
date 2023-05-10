using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript: MonoBehaviour
{
    private ParticleSystem particle;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Play()
    {
        particle = GetComponent<ParticleSystem>();
        transform.SetParent(null);
        particle.Play();
        
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
