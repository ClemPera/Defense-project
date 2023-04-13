using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class Player: MonoBehaviour
{
    private Camera cam;

    public float speed = 7f;

    public float horIn;

    public float verIn;

    private Animator anim;
    
    public TextMeshProUGUI TextA;
    public TextMeshProUGUI TextE;
    public TextMeshProUGUI bonusTextA;
    public TextMeshProUGUI bonusTextE;
    public TextMeshProUGUI regen;
    
    private bool[] availableBonus = new bool[10];

    
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>(); 
        cam = Camera.main;
        
        availableBonus[0] = true;
        availableBonus[1] = true;
        availableBonus[2] = true;
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
        if(Physics.Raycast(ray, out hit)) {
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

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed*verIn * Time.deltaTime);
        transform.position = new Vector3(transform.position.x + speed * horIn * Time.deltaTime, transform.position.y, transform.position.z);
        //transform.Translate(Vector3.forward * speed * verIn);
        if (horIn > 0.01 || horIn < -0.01 || verIn > 0.01 || verIn < -0.01) {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
        
        if(transform.position.z > 24)
            transform.position = new Vector3(transform.position.x, transform.position.y, 24);
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
                StartCoroutine(regenEnCours(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Regen"))
        {
            regen.gameObject.SetActive(false);
            StopCoroutine(regenEnCours(other));
        }
    }

    private IEnumerator regenEnCours(Collider other)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.E))
            {
                transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                String oldRegen = regen.GetParsedText();
                
                regen.SetText("Regeneration en cours");
                StartCoroutine(regeneration());
                
                yield return new WaitForSeconds(5);
                
                regen.SetText(oldRegen);
                break;
            }
            yield return null;
        }
    }
    private IEnumerator regeneration() {
        GameManager.playerHp += 10;
        
        float oldSpeed = speed;
        yield return new WaitForSeconds(0.01f);
        speed = 0;
        
        yield return new WaitForSeconds(5);
        speed = oldSpeed;
    }

    private IEnumerator StopMoving() {
        yield return new WaitForSeconds(5);
        speed = 7;
    }

    private IEnumerator chooseBonus(Collider other)
    {
        int rb, rb2;
        bool ok = false;
        bool ok2 = false;
        KeyCode key = KeyCode.A;
        KeyCode key2 = KeyCode.E;
        
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
            ok  = bonus(rb, bonusTextA, key);
            ok2  = bonus(rb2, bonusTextE, key2);
            yield return null;
        }
        bonusTextA.gameObject.SetActive(false);
        bonusTextE.gameObject.SetActive(false);
        TextA.gameObject.SetActive(false);
        TextE.gameObject.SetActive(false);
        GameManager.bonusValidation = true;

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
        {
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
            text.text = "Autre bonus (WIP)";
                
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }

        return false;
    }
}