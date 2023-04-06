using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject spawnPrefab;
    public GameObject soldierPrefab;
    public int playerHp = 100;
    public float defHp = 1000;
    public Boolean bonusValidation = true;
    public TextMeshProUGUI hpText;

    public GameObject bonusPrefab;
    public float mapSizeXBegin;
    public float mapSizeXEnd;
    public float mapSizeZBegin;
    public float mapSizeZEnd;

    public float projectileInstantiationSpeed = 0.2f;

    public float projectileNumber = 0;
    
    public Texture health1;
    private Image h;

    public Image hfull;
    // Start is called before the first frame update
    void Start()
    {
        Texture h = GameObject.Find("Health1").GetComponent<Image>();
        h = health1;
        StartCoroutine(SpawnWave());
        StartCoroutine(SpawnBonus());
        StartCoroutine(Health());
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "HP : " + defHp;
    }

    IEnumerator SpawnBonus()
    {
        while (true)
        {
            if (bonusValidation == true) //Quand le choix du bonus est validé, lancer le décompte pour un autre bonus
            {
                yield return new WaitForSeconds(30);
                Vector3 spawnPos = new Vector3(Random.Range(mapSizeXBegin, mapSizeXEnd), 1,
                    Random.Range(mapSizeZBegin, mapSizeZEnd));
                Instantiate(bonusPrefab, spawnPos, bonusPrefab.transform.rotation);
                bonusValidation = false;
            }

            yield return null;
        }
    }

    IEnumerator SpawnWave()
    {
        int y = 30;
        while (true)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-45, 45), 0, 40);
            for (int x = 0; x <= y; x++)
            {
                Instantiate(spawnPrefab, spawnPos, spawnPrefab.transform.rotation);
                
                if (x < y / 10) {
                    Instantiate(soldierPrefab, spawnPos, soldierPrefab.transform.rotation);
                }
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(30);
            y += 10;
        }
    }

    IEnumerator Health()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            //Modify Health1 in canvas
            
        }
    }
}
