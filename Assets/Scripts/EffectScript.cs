using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript: MonoBehaviour
{
    private ParticleSystem particleSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        transform.SetParent(null);
        particleSystem.Play();
        
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
