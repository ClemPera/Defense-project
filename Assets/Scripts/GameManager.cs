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

/* Fait par Clément Pera
 * Fait le 07 Mai 2023
 * 
 * Ce script gère une les mécaniques du jeu
 */
public class GameManager : MonoBehaviour
{
    public GameObject araignePrefab; //Prefab de l'ennemie araignée
    public GameObject soldierPrefab; //Prefab de l'ennemie soldier
    
    public static int map; //Numéro de la map
    public static int playerHp; //Vie du joueur
    public static float defHp; //Vie du point de défence
    public static int vagues; //Nombre de vagues
    public static int vaguesSimultanees; //Nombre de vagues simultanées
    public static int maxVagues; //Nombre de vagues maximum
    public int ennemies = 0; //Nombre d'ennemies sur la map
    public static int maxEnnemies; //Nombre d'ennemies maximum
    
    public TextMeshProUGUI hpText; //Texte de la vie du joueur
    public TextMeshProUGUI bonusSpawn; //Texte du bonus
    public TextMeshProUGUI vaguesText; //Texte du nombre de vagues
    public TextMeshProUGUI ennemiesText; //Texte du nombre d'ennemies

    public bool bonusValidation = true; //Booléen de validation si le bonus est pris
    public GameObject bonusPrefab; //Prefab du bonus

    public static float projectileInstantiationSpeed; //Vitesse d'instanciation des projectiles
    public static float projectileNumber; //Nombre de projectiles
    public static int slashDmg; //Dégats du slash
    public static float slashCooldown; //Cooldown du slash

    public static float meteorCooldown; //Cooldown du météore 

    private RawImage health1; //Image du premier coeur de vie du joueur
    private RawImage health2; //Image du deuxième coeur de vie du joueur
    private RawImage health3; //Image du troisième coeur de vie du joueur
    private RawImage health4; //Image du quatrième coeur de vie du joueur
    private RawImage health5; //Image du cinquième coeur de vie du joueur

    public Texture h0; //Texture du coeur vide
    public Texture h25; //Texture du coeur à 25%
    public Texture h50; //Texture du coeur à 50%
    public Texture h75; //Texture du coeur à 75%
    public Texture h100; //Texture du coeur plein

    public Slider defenceHealth; //Slider de la vie du point de défence
    
    private Coroutine spawnWaveCoroutine = null; //Coroutine de spawn des vagues
    private Coroutine spawnBonusCoroutine = null; //Coroutine de spawn des bonus
    
    public Canvas pauseCanvas; //Menu pause
    
    private static AudioSource joueurTouche; //Audio quand le joueur est touché 
    
    // Start is called before the first frame update
    void Start()
    {
        //Si le joueur n'est pas en mode infini, on reset les valeurs
        if (Victoire.victoire == false)
        {
            playerHp = 100;
            defHp = 1000;
            vagues = 0;
            vaguesSimultanees = 1;
            maxVagues = 6; 
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
            maxVagues = 1000;
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
        
        joueurTouche = GetComponent<AudioSource>();
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

    //Fonction si le joueur se fait touché, jouer un son
    public static void Toucher()
    {
        joueurTouche.Play();
    }

    //Coroutine pour mettre en pause le jeu si je joueur appuie sur la touche Echap
    IEnumerator MettreEnPause()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 0)
                {
                    UnPause();
                }else
                {
                    Pause();
                }
            }
            yield return null;
        }
    }

    //Fonction qui met met en pause le jeu
    public void Pause()
    {
        Time.timeScale = 0;
        pauseCanvas.gameObject.SetActive(true);
    }

    //Fonction qui enlève la pause du jeu
    public void UnPause()
    {
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
    //Coroutine qui fait spawn les bonus de manière aléatoire toutes les 30 secondes dès que le joueur a pris le bonus
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

    //Coroutine qui vérifie si le joueur à gagner
    IEnumerator win()
    {
        yield return new WaitForSeconds(10);
        while (true)
        {
            if (vagues == maxVagues)
            {
                StopCoroutine(spawnBonusCoroutine);
                if (ennemies == 0)
                {
                    SceneManager.LoadScene("Victoire");
                }
            }

            yield return null;
        }
    }

    //Coroutine qui vérifie si le joueur à perdu
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
    
    //Coroutine qui affiche le texte de spawn du bonus
    IEnumerator BonusSpawnText()
    {
        bonusSpawn.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        bonusSpawn.gameObject.SetActive(false);
    }

    
    //Coroutine qui fait spawn les nouvelles vagues d'ennemies
    IEnumerator SpawnWave()
    {
        int nombreEnnemies = 30;
        while (true)
        {
            if (vagues != maxVagues)
            {
                Vector3 spawnPos = new Vector3();
                StartCoroutine(augmenterVague());
                //Fait instantier plusieurs vagues en même temps selon le nombre de vagues simultanées
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

                    for (int x = 0; x <= nombreEnnemies; x++)
                    {
                        Instantiate(araignePrefab, spawnPos, araignePrefab.transform.rotation);

                        if (x < nombreEnnemies / 10)
                        {
                            Instantiate(soldierPrefab, spawnPos, soldierPrefab.transform.rotation);
                        }

                        yield return new WaitForSeconds(0.1f);
                    }
                }

                yield return new WaitForSeconds(30);
                nombreEnnemies += 10;
            }
            else
            {
                break;
            }
        }
    }

    //Coroutine qui fait augmenter le nombre de vagues affiché à l'écran
    //et le nombre de vagues simultanées toutes les 5 vagues
    IEnumerator augmenterVague()
    {
        vagues += 1;
        
        if (vagues % 5 == 0)
        {
            vaguesSimultanees += 1;
        }
        
        yield return null;
    }

    //Coroutine qui met à jour la barre de vie du joueur
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