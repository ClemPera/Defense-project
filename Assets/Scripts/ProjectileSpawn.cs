using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* Fait par Clément Pera
 * Fait le 07 Mai 2023
 * 
 * Ce script gère l'apparition des projectiles du joueur
 */

public class ProjectileSpawn : MonoBehaviour
{
    public GameObject projectilePrefab; //Prefab du projectile
    
    public GameObject slashEffect; //Prefab de l'effet du slash
    private Slash slash; //Script de l'effet du slash
    public GameObject slashFront; //Carré gris qui apparait quand le slash est utilisé 
    public TextMeshProUGUI slashTimer; //Timer du slash

    public GameObject meteor; //Prefab du météore
    public GameObject meteorFront; //Carré gris qui apparait quand le météore est utilisé
    public TextMeshProUGUI meteorTimer; //Timer du météore
    private Camera cam; //Caméra principale
    
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

    //Fait apparaitre un météore à l'endroit où le joueur l'active 
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

    //Fait apparaitre le slash devant le joueur et joue l'animation
    IEnumerator Slash()
    {
        while (true)
        {
            if (Input.GetButton("Fire2"))
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

    //Fait apparaitre un projectile devant le joueur
    IEnumerator Projectile1()
    {
        while (true)
        {
            if (Input.GetButton("Fire1"))
            {
                Instantiate(projectilePrefab, new Vector3 (transform.position.x, 1, transform.position.z) , transform.rotation);
                yield return new WaitForSeconds(GameManager.projectileInstantiationSpeed);
            }

            yield return null;
        }
    }
}