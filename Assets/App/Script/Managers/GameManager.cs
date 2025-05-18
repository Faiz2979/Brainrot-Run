using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private bool isPlaying;
    public bool IsPlaying => isPlaying; // this variable can used for global

    private void Awake()
    {
        // for make sure the GameManager is not duplicate
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    public void SetIsPlaying(bool SetIsPlaying)
    {
        this.isPlaying = SetIsPlaying;
    }
}
