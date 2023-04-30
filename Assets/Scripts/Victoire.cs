using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Victoire : MonoBehaviour
{
    public static bool victoire = false;

    public TextMeshProUGUI vagues;
    public TextMeshProUGUI ennemies;
    
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