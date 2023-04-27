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

    public void map1() {
        SceneManager.LoadScene("Main Scene");
        GameManager.map = 1;
    }
    
    public void map2() {
        SceneManager.LoadScene("Scene 2");
        GameManager.map = 2;
    }
    
    public void menu() {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    //Ajouter mode infini qui load la bonne map
    
    public void infini()
    {
        GameManager.maxVagues = 1000;
        if (GameManager.map == 1)
            SceneManager.LoadScene("Main Scene");
        else if (GameManager.map == 2)
            SceneManager.LoadScene("Scene 2");
    }
}