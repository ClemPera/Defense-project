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
    public TextMeshProUGUI bonusSpawn;

    public GameObject bonusPrefab;
    public float mapSizeXBegin;
    public float mapSizeXEnd;
    public float mapSizeZBegin;
    public float mapSizeZEnd;

    public float projectileInstantiationSpeed = 0.2f;

    public float projectileNumber = 0;

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
    // Start is called before the first frame update
    void Start()
    {
        health1 = GameObject.Find("Health1").GetComponent<RawImage>();
        health2 = GameObject.Find("Health2").GetComponent<RawImage>();
        health3 = GameObject.Find("Health3").GetComponent<RawImage>();
        health4 = GameObject.Find("Health4").GetComponent<RawImage>();
        health5 = GameObject.Find("Health5").GetComponent<RawImage>();
        StartCoroutine(SpawnWave());
        StartCoroutine(SpawnBonus());
        StartCoroutine(Health());
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = defHp + "/1000";
        defenceHealth.maxValue = 1000;
        defenceHealth.value = defHp;
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
                StartCoroutine(BonusSpawnText());
                bonusValidation = false;
            }

            yield return null;
        }
    }

    IEnumerator BonusSpawnText(){
        bonusSpawn.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        bonusSpawn.gameObject.SetActive(false);
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
