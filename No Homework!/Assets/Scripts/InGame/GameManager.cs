using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private Startmethod startmethod;

    [SerializeField]
    private bool isGameActive;
    private bool offlineOverride = false;

    public static bool IsGameActive
    {
        get { return Instance.isGameActive; }
    }
    public static bool IsMultiplayer
    {
        get { if (Instance.startmethod == Startmethod.multiplayer) { return true; } else { return false; } }
    }
    public static bool IsSingleplayer
    {
        get { if (Instance.startmethod == Startmethod.singleplayer) { return true; } else { return false; } }
    }
    public static bool IsOffline
    {
        get { if (Instance.startmethod == Startmethod.offline) { return true; } else { return false; } }
    }

    public enum Startmethod
    {
        singleplayer,
        multiplayer,
        offline
    }

    //Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void StartGame(Startmethod _method)
    {
        Instance.startmethod = _method;

        if (Instance.offlineOverride)
            Instance.startmethod = Startmethod.offline;

        AudioManager.Instance.Stop("MainMenuMusic");
        Instance.isGameActive = true;

        SceneManager.LoadScene(GameConstants.GAME_SCENE);
    }

    public static void EndGame(bool _won)
    {
        if (_won)
        {

        } 
        else
        {
            instance.isGameActive = false;
            SceneManager.LoadScene(GameConstants.MAIN_MENU_SCENE);
        }
    }

    public static void PlayOffline()
    {
        Instance.offlineOverride = true;

        //Show our next screen if we logged in successfully.
        SceneManager.LoadScene(GameConstants.MAIN_MENU_SCENE);
    }
}
