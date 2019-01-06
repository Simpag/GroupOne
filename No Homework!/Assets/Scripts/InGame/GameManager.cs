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

    [SerializeField]
    private bool isMultiplayer;
    private bool isGameActive;

    public static bool IsMultiplayer
    {
        get { return Instance.isMultiplayer; }
    }
    public static bool IsGameActive
    {
        get { return Instance.isGameActive; }
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

    public static void StartGame(bool _isMulti)
    {
        AudioManager.Instance.Stop("MainMenuMusic");
        Instance.isMultiplayer = _isMulti;
        SceneManager.LoadScene(GameConstants.GAME_SCENE);

        Instance.isGameActive = true;
    }

    public static void EndGame()
    {
        instance.isGameActive = false;
    }
}
