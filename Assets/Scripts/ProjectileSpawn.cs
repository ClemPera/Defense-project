using System.Collections;
using System.Collections.Generic;
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
                Instantiate(projectilePrefab, transform.position, transform.rotation);
            }
            yield return new WaitForSeconds(GameManager.projectileInstantiationSpeed);
        }
    }
}