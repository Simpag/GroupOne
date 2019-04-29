using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class LogInUIManager : MonoBehaviour {

    public enum Menu
    {
        MainMenu,
        ShooterMenu,
        CollectionMenu,
        MarketMenu,
        VendingMachineMenu
    }

    public AccountInfo info;

    private static LogInUIManager instance;
    public static LogInUIManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public Button RESET;

    [Header("Banner")]
    [SerializeField]
    private Text coins;
    public static Text Coins
    {
        get { return Instance.coins; }
        set { Instance.coins = value; }
    }
    [SerializeField]
    private Text playerName;
    public static Text PlayerName
    {
        get { return Instance.playerName; }
        set { Instance.playerName = value; }
    }

    [Header("Menus")]
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject shooterMenu;
    [SerializeField]
    private GameObject collectionMenu;
    [SerializeField]
    private GameObject marketMenu;
    [SerializeField]
    private GameObject vendingMachineMenu;

    //Collection
    List<GameObject> collectionItems; //Create a list of instantiated collection items

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        info = FindObjectOfType<AccountInfo>();

        SwitchMenu(Menu.MainMenu);
    }

    private void Update()
    {
        if (info.DisplayName == null)
            return;
        UpdateText();

    }

    private void UpdateText()
    {
        PlayerName.text = info.DisplayName;

        float? _coins = -1;

        if(info.Currency != null)
        {
            _coins = info.Currency;
        }

        Coins.text = _coins.ToString();
    }

    public void SwitchMenu(Menu _menu)
    {
        switch (_menu)
        {
            case Menu.MainMenu:
                mainMenu.SetActive(true);
                shooterMenu.SetActive(false);
                collectionMenu.SetActive(false);
                marketMenu.SetActive(false);
                vendingMachineMenu.SetActive(false);

                AccountInfo.UpdateAccountInfo();

                break;
            case Menu.ShooterMenu:
                mainMenu.SetActive(false);
                shooterMenu.SetActive(true);
                collectionMenu.SetActive(false);
                marketMenu.SetActive(false);
                vendingMachineMenu.SetActive(false);
                break;
            case Menu.CollectionMenu:
                //UI
                mainMenu.SetActive(false);
                shooterMenu.SetActive(false);
                collectionMenu.SetActive(true);
                marketMenu.SetActive(false);
                vendingMachineMenu.SetActive(false);

                break;
            case Menu.MarketMenu:
                //UI
                mainMenu.SetActive(false);
                shooterMenu.SetActive(false);
                collectionMenu.SetActive(false);
                marketMenu.SetActive(true);
                vendingMachineMenu.SetActive(false);

                break;
            case Menu.VendingMachineMenu:
                mainMenu.SetActive(false);
                shooterMenu.SetActive(false);
                collectionMenu.SetActive(false);
                marketMenu.SetActive(false);
                vendingMachineMenu.SetActive(true);
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
                SwitchMenu(Menu.ShooterMenu);
                break;
            case 2:
                SwitchMenu(Menu.CollectionMenu);
                break;
            case 3:
                SwitchMenu(Menu.MarketMenu);
                break;
            case 4:
                SwitchMenu(Menu.VendingMachineMenu);
                break;
        }
    }
}
