using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFunctions {

    public static bool IsGamePaused = false;

    //public static void OnPlayFaberror(PlayFabError error)
    //{
    //    Debug.LogError(error.GenerateErrorReport());
    //}

    public static void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0f;
    }

    public static void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;
    }
}
