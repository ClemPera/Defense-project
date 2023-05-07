using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bouton : MonoBehaviour
{
    
    //Quitte le jeu
    public void Quitter(){
        Application.Quit();
        Debug.Log("Fermeture du jeu");
    }

    //Joue la map 1
    public void Map1() {
        SceneManager.LoadScene("Main Scene");
        GameManager.map = 1;
    }
    
    //Joue la map 2
    public void Map2() {
        SceneManager.LoadScene("Scene 2");
        GameManager.map = 2;
    }
    
    //Lance la scène Menu
    public void Menu() {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    
    
    //Joue le mode infini
    public void Infini()
    {
        GameManager.maxVagues = 1000;
        if (GameManager.map == 1)
            SceneManager.LoadScene("Main Scene");
        else if (GameManager.map == 2)
            SceneManager.LoadScene("Scene 2");
    }
}