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
        InvokeRepeating("UpdateTowerTargeting", 0.5f, 0.5f);
    }

    public static void StartGame(bool _isMulti)
    {
        AudioManager.Instance.Stop("MainMenuMusic");
        Instance.isMultiplayer = _isMulti;
        SceneManager.LoadScene(GameConstants.GAME_SCENE);

        Instance.isGameActive = true;
        Instance.StartCoroutine("UpdateTowerTargeting");
    }

    public static void EndGame()
    {
        instance.isGameActive = false;
    }

    private IEnumerator UpdateTowerTargeting()
    {
        yield return new WaitForSeconds(1f); //Give it some time to load fully

        while(IsGameActive)
        {
            yield return new WaitForSeconds(0.5f);
            TowerRange.UpdateTowerTargetList();
            //Debug.Log("This should take .5 secounds");
        }
    }
}
