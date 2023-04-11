using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierProjectile : MonoBehaviour
{
    public float speed = 30f;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
    
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            gameManager.playerHp -= 1;
            Destroy(gameObject);
        }
    }
}
