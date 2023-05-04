using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 30f;
    
    public GameObject explosionEffect;
    private EffectScript effect;
    
    public AudioClip shotSound;

    private AudioSource playerAudio;

    public float shotVolume = 0.7f;

    public GameObject explodeProjectile;

    // Start is called before the first frame update
    void Start()
    {
        effect = explosionEffect.GetComponent<EffectScript>();
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
        if (other.gameObject.GetComponent<Enemy>() != null || other.gameObject.GetComponent<Soldier>() != null)
        {
            for (int i = 0; i < GameManager.projectileNumber; i += 1)
            {
                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.y += Random.Range(0, 360);
                Instantiate(explodeProjectile, transform.position, Quaternion.Euler(rotationVector));
            }

            if (other.gameObject.GetComponent<Enemy>() != null)
                other.gameObject.GetComponent<Enemy>().health -= 1;
            else if (other.gameObject.GetComponent<Soldier>() != null)
                other.gameObject.GetComponent<Soldier>().health -= 1;

            effect.Play();
            Destroy(gameObject);
        }
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