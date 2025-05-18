using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctionManager : MonoBehaviour
{

    public void PlayGame(){
        Debug.Log("Play Game");
        // play game logic here
        ScoreManager.Instance.StartScoring(); // Mulai hitung skor
    }

    public void OpenShop(){
        Debug.Log("Open Shop");
        // open shop logic here

    }

    public void QuitGame(){
        Debug.Log("Quit Game");
        Application.Quit();
    }

}
