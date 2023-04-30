using System.Collections;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    public GameObject projectilePrefab;
    
    public GameObject slashEffect;
    private Slash slash;
    // Start is called before the first frame update
    void Start()
    {
        slash = slashEffect.GetComponent<Slash>();
        
        StartCoroutine(Projectile1());
        StartCoroutine(Slash());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Slash()
    {
        while (true)
        {
            if (Input.GetMouseButton(1))
            {
                slashEffect.SetActive(true);
                slash.Play();
                yield return new WaitForSeconds(0.5f);
                slashEffect.SetActive(false);
                yield return new WaitForSeconds(7.5f);
            }

            yield return null;
        }
    }

    IEnumerator Projectile1()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                Instantiate(projectilePrefab, new Vector3 (transform.position.x, 1, transform.position.z) , transform.rotation);
            }
            yield return new WaitForSeconds(GameManager.projectileInstantiationSpeed);
        }
    }
}