using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victoire : MonoBehaviour
{
    public static bool victoire = false;
    public static int playerHp = 0;
    public static float defHp = 0;
    public static int vagues = 0;
    public static int maxVagues = 1000;
    public static int maxEnnemies = 0;
    public static float projectileInstantiationSpeed = 0;
    public static float projectileNumber = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        playerHp = GameManager.playerHp;
        defHp = GameManager.defHp;
        vagues = GameManager.vagues;
        maxVagues = 1000;
        maxEnnemies = GameManager.maxEnnemies;
        projectileInstantiationSpeed = GameManager.projectileInstantiationSpeed;
        projectileNumber = GameManager.projectileNumber;
        victoire = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}