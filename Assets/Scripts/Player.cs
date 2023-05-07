using System;
using System.Collections;
using System.Numerics;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    private Camera cam;

    public static float speed = 7f;
    public float horIn;
    public float verIn;

    private Animator anim;

    public TextMeshProUGUI TextA;
    public TextMeshProUGUI TextE;
    public TextMeshProUGUI bonusTextA;
    public TextMeshProUGUI bonusTextE;
    
    public TextMeshProUGUI regen;
    public ParticleSystem healing;
    
    public Coroutine regenEnCoursCoroutine = null;

    private bool[] availableBonus = new bool[10];

    private GameManager gameManager;
    
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        availableBonus[0] = true;
        //availableBonus[1] = true;
        availableBonus[2] = true;
        availableBonus[3] = true;
        availableBonus[4] = true;
        availableBonus[5] = true;
    }

    // Update is called once per frame
    private void Update()
    {
        followCursor();
        move();
    }

    private void followCursor()
    {
        //Look at direction : https://answers.unity.com/questions/731796/how-to-project-the-mouse-cursor-into-3d-space-for.html

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
        else
        {
            transform.LookAt(new Vector3(ray.origin.x + ray.direction.x, 0, 0));
        }
    }

    private void move()
    {
        horIn = Input.GetAxis("Horizontal");
        verIn = Input.GetAxis("Vertical");
        //anim.SetFloat("Speed", verIn);

        transform.position = new Vector3(transform.position.x, transform.position.y,
            transform.position.z + speed * verIn * Time.deltaTime);
        transform.position = new Vector3(transform.position.x + speed * horIn * Time.deltaTime, transform.position.y,
            transform.position.z);
        //transform.Translate(Vector3.forward * speed * verIn);
        if (horIn > 0.01 || horIn < -0.01 || verIn > 0.01 || verIn < -0.01)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        //Mur invisible
        if (GameManager.map == 1)
        {
            if (transform.position.z > 24)
                transform.position = new Vector3(transform.position.x, transform.position.y, 24);
        }
        else if (GameManager.map == 2)
        {
            if (transform.position.x > 39)
                transform.position = new Vector3(39, transform.position.y, transform.position.z);
            if (transform.position.z > 29)
                transform.position = new Vector3(transform.position.x, transform.position.y, 29);
            if (transform.position.x < -29)
                transform.position = new Vector3(-29, transform.position.y, transform.position.z);
            if (transform.position.z < -39)
                transform.position = new Vector3(transform.position.x, transform.position.y, -39);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            StartCoroutine(chooseBonus(other));
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Regen"))
        {
            if (GameManager.playerHp <= 90)
            {
                regen.gameObject.SetActive(true);
                regenEnCoursCoroutine = StartCoroutine(regenEnCours(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Regen"))
        {
            regen.gameObject.SetActive(false);
            
            if(regenEnCoursCoroutine != null)
                StopCoroutine(regenEnCoursCoroutine);
        }
    }

    private IEnumerator regenEnCours(Collider other)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                transform.position = new Vector3(other.transform.position.x, transform.position.y,
                    other.transform.position.z);
                
                StartCoroutine(regeneration());

                yield return new WaitForSeconds(3.5f);
                
                healing.Stop();

                break;
            }

            yield return null;
        }
    }

    private IEnumerator regeneration()
    {
        healing.Play();

        float oldSpeed = speed;
        yield return new WaitForSeconds(0.01f);
        speed = 0;

        yield return new WaitForSeconds(5);
        GameManager.playerHp += 10;
        speed = oldSpeed;
    }

    private IEnumerator chooseBonus(Collider other)
    {
        int rb, rb2;
        bool ok = false;
        bool ok2 = false;
        KeyCode key = KeyCode.Alpha1;
        KeyCode key2 = KeyCode.Alpha2;

        do
        {
            rb = UnityEngine.Random.Range(0, availableBonus.Length);
        } while (availableBonus[rb] == false);

        do
        {
            rb2 = UnityEngine.Random.Range(0, availableBonus.Length);
        } while (availableBonus[rb2] == false || rb2 == rb);

        bonusTextA.gameObject.SetActive(true);
        bonusTextE.gameObject.SetActive(true);
        TextA.gameObject.SetActive(true);
        TextE.gameObject.SetActive(true);
        while (!ok && !ok2)
        {
            ok = bonus(rb, bonusTextA, key);
            ok2 = bonus(rb2, bonusTextE, key2);
            yield return null;
        }

        bonusTextA.gameObject.SetActive(false);
        bonusTextE.gameObject.SetActive(false);
        TextA.gameObject.SetActive(false);
        TextE.gameObject.SetActive(false);
        gameManager.bonusValidation = true;

    }

    private bool bonus(int b, TextMeshProUGUI text, KeyCode key)
    {
        if (b == 0)
        {
            text.text = "Vitesse des projectiles";

            if (Input.GetKeyDown(key))
            {
                GameManager.projectileInstantiationSpeed -= (GameManager.projectileInstantiationSpeed * 10) / 100;
                return true;
            }
        }

        else if (b == 1)
        { //Bonus fait tout planter 
            text.text = "Balle rebondissantes";

            if (Input.GetKeyDown(key))
            {
                GameManager.projectileNumber = 1;
                availableBonus[1] = false;

                return true;
            }
        }
        else if (b == 2)
        {
            text.text = "Vitesse de déplacement";

            if (Input.GetKeyDown(key))
            {
                speed += (speed * 10) / 100;
                return true;
            }
        }
        else if (b == 3)
        {
            text.text = "Dégats du Slash (clique droit)";
            
            if (Input.GetKeyDown(key))
            {
                GameManager.slashDmg += 1;
                return true;
            }
        }
        else if (b == 4)
        {
            text.text = "Réd. de la recharge du Slash"; 
            if (Input.GetKeyDown(key))
            {
                GameManager.slashCooldown -= 1;
                return true;
            }
        }
    else if (b == 5)
        {
            text.text = "Réd. de la recharge du metéore"; 
            if (Input.GetKeyDown(key))
            {
                GameManager.meteorCooldown -= 1;
                return true;
            }
        }

        return false;
    }
}