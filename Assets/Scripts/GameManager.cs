using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public GameObject spawnPrefab;
    public GameObject soldierPrefab;
    public static int map;
    public static int playerHp = 100;
    public static float defHp = 1000;
    public static int vagues = 0;
    public static int vaguesSimultanees = 1;
    public static int maxVagues = 10;
    public int ennemies = 0;
    public static int maxEnnemies = 0;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI bonusSpawn;
    public TextMeshProUGUI vaguesText;
    public TextMeshProUGUI ennemiesText;

    public bool bonusValidation = true;
    public GameObject bonusPrefab;

    public static float projectileInstantiationSpeed = 0.2f;
    public static float projectileNumber = 0;
    public static int slashDmg;
    public static float slashCooldown;

    public static float meteorCooldown;
    public static float meteorTime;

    private RawImage health1;
    private RawImage health2;
    private RawImage health3;
    private RawImage health4;
    private RawImage health5;

    public Texture h0;
    public Texture h25;
    public Texture h50;
    public Texture h75;
    public Texture h100;

    public Slider defenceHealth;
    
    private Coroutine spawnWaveCoroutine = null;
    private Coroutine spawnBonusCoroutine = null;
    
    public Canvas pauseCanvas;
    
    private static AudioSource source;
    public static AudioClip shotSound; 
    public static float shotVolume = 0.8f;


    // Start is called before the first frame update
    void Start()
    {
        if (Victoire.victoire == false)
        {
            playerHp = 100;
            defHp = 1000;
            vagues = 0;
            vaguesSimultanees = 1;
            maxVagues = 10;
            maxEnnemies = 0;
            projectileInstantiationSpeed = 0.2f;
            projectileNumber = 0;
            slashDmg = 1;
            slashCooldown = 6;
            meteorCooldown = 12;
        }
        else
        {
            Victoire.victoire = false;
        }
        
        health1 = GameObject.Find("Health1").GetComponent<RawImage>();
        health2 = GameObject.Find("Health2").GetComponent<RawImage>();
        health3 = GameObject.Find("Health3").GetComponent<RawImage>();
        health4 = GameObject.Find("Health4").GetComponent<RawImage>();
        health5 = GameObject.Find("Health5").GetComponent<RawImage>();
        spawnWaveCoroutine = StartCoroutine(SpawnWave());
        spawnBonusCoroutine = StartCoroutine(SpawnBonus());
        StartCoroutine(Health());
        StartCoroutine(win());
        StartCoroutine(lose());
        StartCoroutine(MettreEnPause());
        
        source = GetComponent<AudioSource>();
        shotSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sounds/Toucher.mp3", typeof(AudioClip));
    }

    // Update is called once per frame
    void Update()
    {
        vaguesText.text = "Vagues: " + vagues + "/" + maxVagues;
        ennemiesText.text = "Ennemies: " + ennemies;
        hpText.text = defHp + "/1000";
        defenceHealth.maxValue = 1000;
        defenceHealth.value = defHp;
    }

    public static void sonToucher()
    {
        source.PlayOneShot(shotSound, shotVolume);
    }
    
    IEnumerator MettreEnPause()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale != 0)
                {
                    Pause();
                }else
                {
                    UnPause();
                }
            }
            yield return null;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseCanvas.gameObject.SetActive(true);
    }

    public void UnPause()
    {
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
    

    IEnumerator SpawnBonus()
    {
        while (true)
        {
            if (bonusValidation) //Quand le choix du bonus est validé, lancer le décompte pour un autre bonus
            {
                yield return new WaitForSeconds(20);
                Vector3 spawnPos = new Vector3();
                if (map == 1)
                {
                    spawnPos = new Vector3(Random.Range(-39, 22), 1,
                        Random.Range(23, 7));
                }else if (map == 2) 
                {
                    spawnPos = new Vector3(Random.Range(-28, 38), 1,
                        Random.Range(28, -38));
                }

                Instantiate(bonusPrefab, spawnPos, bonusPrefab.transform.rotation);
                StartCoroutine(BonusSpawnText());
                bonusValidation = false;
            }

            yield return null;
        }
    }

    IEnumerator win()
    {
        yield return new WaitForSeconds(10);
        while (true)
        {
            if (vagues == maxVagues)
            {
                StopCoroutine(spawnWaveCoroutine);
                StopCoroutine(spawnBonusCoroutine);
                if (ennemies == 0)
                {
                    SceneManager.LoadScene("Victoire");
                }
            }

            yield return null;
        }
    }

    IEnumerator lose()
    {
        while (true)
        {
            if (defHp <= 0 || playerHp <= 0)
            {
                SceneManager.LoadScene("Defaite");
            }

            yield return null;
        }
    }
    
    IEnumerator BonusSpawnText()
    {
        bonusSpawn.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        bonusSpawn.gameObject.SetActive(false);
    }

    IEnumerator SpawnWave()
    {
        int y = 30;
        while (true)
        {
            Vector3 spawnPos = new Vector3();
            StartCoroutine(augmenterVague());
            for (int i = 0; i < vaguesSimultanees; i++)
            {
                if (map == 1)
                {
                    spawnPos = new Vector3(Random.Range(-45, 45), 0, 40);
                }
                else if (map == 2)
                {
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            spawnPos = new Vector3(-41, 0, Random.Range(-41, 31));
                            break;
                        case 1:
                            spawnPos = new Vector3(41, 0, Random.Range(-41, 31));
                            break;
                        case 2:
                            spawnPos = new Vector3(Random.Range(-41, 41), 0, 41);
                            break;
                        case 3:
                            spawnPos = new Vector3(Random.Range(-41, 41), 0, -51);
                            break;
                    }
                }

                for (int x = 0; x <= y; x++)
                {
                    Instantiate(spawnPrefab, spawnPos, spawnPrefab.transform.rotation);

                    if (x < y / 10)
                    {
                        Instantiate(soldierPrefab, spawnPos, soldierPrefab.transform.rotation);
                    }

                    yield return new WaitForSeconds(0.1f);
                }
            }

            yield return new WaitForSeconds(30);
            y += 10;
        }
    }

    IEnumerator augmenterVague()
    {
        yield return new WaitForSeconds(0.5f);
        vagues += 1;
        
        if (vagues % 5 == 0)
        {
            vaguesSimultanees += 1;
        }
    }

    IEnumerator Health()
    {
        while (true)
        {
            if (playerHp == 100) {
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h100;
                health4.texture = h100;
                health5.texture = h100;
            } else if (playerHp >= 95) {
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h100;
                health4.texture = h100;
                health5.texture = h75;
            } else if (playerHp >= 90) {
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h100;
                health4.texture = h100;
                health5.texture = h50;
            } else if (playerHp >= 85) {
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h100;
                health4.texture = h100;
                health5.texture = h25;
            } else if (playerHp >= 80) {
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h100;
                health4.texture = h100;
                health5.texture = h0;
            } else if (playerHp >= 75) {
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h100;
                health4.texture = h75;
                health5.texture = h0;
            } else if (playerHp >= 70){
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h100;
                health4.texture = h50;
                health5.texture = h0;
            } else if (playerHp >= 65){
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h100;
                health4.texture = h25;
                health5.texture = h0;
            } else if (playerHp >= 60){
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h100;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 55){
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h75;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 50){
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h50;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 45){
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h25;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 40){
                health1.texture = h100;
                health2.texture = h100;
                health3.texture = h0;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 35){
                health1.texture = h100;
                health2.texture = h75;
                health3.texture = h0;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 30){
                health1.texture = h100;
                health2.texture = h50;
                health3.texture = h0;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 25){
                health1.texture = h100;
                health2.texture = h25;
                health3.texture = h0;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 20){
                health1.texture = h100;
                health2.texture = h0;
                health3.texture = h0;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 15){
                health1.texture = h75;
                health2.texture = h0;
                health3.texture = h0;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 10){
                health1.texture = h50;
                health2.texture = h0;
                health3.texture = h0;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp >= 5){
                health1.texture = h25;
                health2.texture = h0;
                health3.texture = h0;
                health4.texture = h0;
                health5.texture = h0;
            } else if (playerHp == 0){
                health1.texture = h0;
                health2.texture = h0;
                health3.texture = h0;
                health4.texture = h0;
                health5.texture = h0;
            }
            yield return null; 
        }
    }
}