using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctionManager : MonoBehaviour
{

    public void PlayGame(){
        Debug.Log("Play Game");
        // play game logic here
        GameManager.Instance.SetIsPlaying(true);
    }

    public void OpenShop(){
        Debug.Log("Open Shop");
        // open shop logic here

    }

    public void QuitGame(){
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void PauseGame(){
        Debug.Log("Pause Game");
        // pause game logic here
        GameManager.Instance.SetIsPlaying(false);
    }

    public void ResumeGame(){
        Debug.Log("Resume Game");
        // resume game logic here
        GameManager.Instance.SetIsPlaying(true);
    }

    public void RestartGame(){
        Debug.Log("Restart Game");
        // restart game logic here
        GameManager.Instance.SetIsPlaying(false);
        ScoreManager.Instance.ResetScore();
        GameManager.Instance.SetIsPlaying(true);
    }

}
