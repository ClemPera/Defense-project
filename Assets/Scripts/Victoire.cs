using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victoire : MonoBehaviour
{
    public static int Hp;
    // Start is called before the first frame update
    void Start()
    {
        Hp = GameManager.playerHp;
        Debug.Log(Hp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
