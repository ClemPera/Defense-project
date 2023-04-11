using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Projectile : MonoBehaviour
{
    public float speed = 30f;
    
    public AudioClip shotSound;

    private AudioSource playerAudio;

    public float shotVolume = 0.7f;

    public GameObject explodeProjectile;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerAudio = GetComponent<AudioSource>();
        playerAudio.PlayOneShot(shotSound, shotVolume);
        StartCoroutine(deleteProjectile());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i= 0; i < gameManager.projectileNumber; i += 1)
        {
            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.y += UnityEngine.Random.Range(0, 360);
            Instantiate(explodeProjectile, transform.position, Quaternion.Euler(rotationVector));
        }

        if(other.gameObject.GetComponent<Enemy>() != null)
            other.gameObject.GetComponent<Enemy>().health -= 1;
        else if(other.gameObject.GetComponent<Soldier>() != null)
            other.gameObject.GetComponent<Soldier>().health -= 1;
            
        
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    IEnumerator deleteProjectile()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}