using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public enum Menu
    {
        MainMenu,
        StoreMenu
    }

    public AccountInfo info;

    private static MainMenuManager instance;
    public static MainMenuManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [SerializeField]
    private Animator anim;

    [Header("Menus")]
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject storeMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        info = FindObjectOfType<AccountInfo>();
    }

    private void Start()
    {
        StoreManager.HideStore();
        AccountInfo.UpdateAccountInfo();
    }

    public void SwitchMenu(Menu _menu)
    {
        switch (_menu)
        {
            case Menu.MainMenu:
                anim.SetTrigger("FlipBoardFront");

                StoreManager.HideStore();
                AccountInfo.UpdateAccountInfo();
                break;

            case Menu.StoreMenu:
                anim.SetTrigger("FlipBoardBack");

                StoreManager.ShowStore();
                break;
        }
    }

    public void ButtonSwitchMenu(int _menu)
    {
        switch (_menu)
        {
            case 0:
                SwitchMenu(Menu.MainMenu);
                break;
            case 1:
                SwitchMenu(Menu.StoreMenu);
                break;
        }
    }

    public void PlaySingleplayer ()
    {
        GameManager.SetupGameMode(GameManager.Startmethod.singleplayer);
        SceneManager.LoadScene(GameConstants.PRE_GAME_SCENE);
    }

    public void PlayMultiplayer ()
    {
        GameManager.SetupGameMode(GameManager.Startmethod.multiplayer);
        SceneManager.LoadScene(GameConstants.PRE_GAME_SCENE);
    }
}
