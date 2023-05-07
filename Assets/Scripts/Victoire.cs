using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/* Fait par Clément Pera
 * Fait le 07 Mai 2023
 * 
 * Ce script gère la victoire du joueur 
 */
public class Victoire : MonoBehaviour
{
    public static bool victoire = false; //Défini si le joueur a gagné ou non

    public TextMeshProUGUI vagues; //Nombre de vagues
    public TextMeshProUGUI ennemies; //Nombre d'ennemies tuées
    
    // Start is called before the first frame update
    void Start()
    {
        victoire = true;
        
        vagues.text = "Nombre de vagues : " + GameManager.vagues;
        
        //Nombre d'ennemies tuées
        ennemies.text = "Nombre d'ennemies tués : " + GameManager.maxEnnemies;
    }

    // Update is called once per frame
    void Update()
    {
    }
}