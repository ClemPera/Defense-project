using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/* Fait par Clément Pera
 * Fait le 07 Mai 2023
 * 
 * Ce script gère les projectiles de l'ennemie soldier 
 */
public class SoldierProjectile : MonoBehaviour
{
    public float speed = 30f; //Vitesse du projectile
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (transform.position.x < -100 || transform.position.x > 100 || transform.position.z < -100 ||
            transform.position.z > 100)
        {
            Destroy(gameObject);
        }
        
    }
    
    //Si le projectile touche un mur, il est détruit et s'il touche le joueur, il perd de la vie
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            GameManager.Toucher();
            GameManager.playerHp -= 1;
            Cam.Shake();
            Destroy(gameObject);
        }
    }
}
