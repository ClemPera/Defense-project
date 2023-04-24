using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Defaite : MonoBehaviour
{
    public TextMeshProUGUI vagues;
    public TextMeshProUGUI ennemies;
    // Start is called before the first frame update
    void Start()
    {
        //Nombre de vagues
        vagues.text = "Nombre de vagues : " + GameManager.vagues;
        
        //Nombre d'ennemies tuées
        ennemies.text = "Nombre d'ennemies tués : " + GameManager.maxEnnemies;
        Victoire.victoire = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
