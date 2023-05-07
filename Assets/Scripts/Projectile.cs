using System.Collections;
using UnityEngine;

/* Fait par Clément Pera
 * Fait le 07 Mai 2023
 * 
 * Ce script gère les projectiles du joueur
 */
public class Projectile : MonoBehaviour
{
    public float speed = 30f; //vitesse du projectile
    
    public GameObject explosionEffect; //objet de l'effet de l'explosion
    private EffectScript effect; //script de l'effet de l'explosion
    
    private AudioSource playerAudio; //source audio du joueur

    public GameObject explodeProjectile; //objet de l'explosion du projectile

    // Start is called before the first frame update
    void Start()
    {
        effect = explosionEffect.GetComponent<EffectScript>();
        playerAudio = GetComponent<AudioSource>();
        playerAudio.Play();
        StartCoroutine(deleteProjectile());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    //si le projectile touche un objet trigger et que c'est une araignée ou un soldat, on lui enlève de la vie et on détruit le projectile
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Araigne>() != null || other.gameObject.GetComponent<Soldier>() != null)
        {
            //On réinstantie des projectiles si il y a le bonus de rebondissement
            for (int i = 0; i < GameManager.projectileNumber; i += 1)
            {
                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.y += Random.Range(0, 360);
                Instantiate(explodeProjectile, transform.position, Quaternion.Euler(rotationVector));
            }

            if (other.gameObject.GetComponent<Araigne>() != null)
                other.gameObject.GetComponent<Araigne>().health -= 1;
            else if (other.gameObject.GetComponent<Soldier>() != null)
                other.gameObject.GetComponent<Soldier>().health -= 1;

            effect.Play();
            Destroy(gameObject);
        }
    }

    //si le projectile touche un objet, on détruit le projectile si c'est un mur
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    
    //cette coroutine détruit le projectile au bout de 3 secondes
    IEnumerator deleteProjectile()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}