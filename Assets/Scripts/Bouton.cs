using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bouton : MonoBehaviour
{
    public void quitter(){
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void jouer() {
        SceneManager.LoadScene("Main Scene");
        GameManager.map = 1;
    }
    
    public void menu() {
        SceneManager.LoadScene("Menu");
    }
}