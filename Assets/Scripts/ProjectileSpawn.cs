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

    public GameObject meteor;
    public GameObject meteorFront;
    public TextMeshProUGUI meteorTimer;
    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        slash = slashEffect.GetComponent<Slash>();
        slashEffect.SetActive(false);
        
        StartCoroutine(Projectile1());
        StartCoroutine(Slash());
        StartCoroutine(Meteor());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Meteor()
    {
        while (true){
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Instantiate(meteor, new Vector3(hit.point.x, 1, hit.point.z), Quaternion.identity);
                    meteorFront.SetActive(true);
                    meteorTimer.text =(GameManager.meteorCooldown).ToString();
                    for (int i = 1; i < GameManager.meteorCooldown; i++)
                    {
                        meteorTimer.text = (GameManager.meteorCooldown - i).ToString();
                        yield return new WaitForSeconds(1f);
                    }
                    meteorTimer.text = "";
                    meteorFront.SetActive(false);
                }
            }

            yield return null;
        }
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
                yield return new WaitForSeconds(GameManager.projectileInstantiationSpeed);
            }

            yield return null;
        }
    }
}