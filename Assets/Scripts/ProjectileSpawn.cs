using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileSpawn : MonoBehaviour
{
    public GameObject projectilePrefab;
    
    public GameObject slashEffect;
    private Slash slash;
    public GameObject slashFront;
    public TextMeshProUGUI slashTimer;
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
                
                slashTimer.text =(GameManager.slashCooldown-1).ToString();
                slashFront.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                
                slashEffect.SetActive(false);
                
                for (int i = 1; i < GameManager.slashCooldown; i++)
                {
                    slashTimer.text = (GameManager.slashCooldown - i).ToString();
                    yield return new WaitForSeconds(1f);
                }
                slashTimer.text = "";
                slashFront.SetActive(false);
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