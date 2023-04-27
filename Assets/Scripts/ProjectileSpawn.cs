using System.Collections;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    public GameObject projectilePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(projectile1());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator projectile1()
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