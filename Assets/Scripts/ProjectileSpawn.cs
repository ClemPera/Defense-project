using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    public GameObject projectilePrefab;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
            yield return new WaitForSeconds(gameManager.projectileInstantiationSpeed);
        }
    }
}