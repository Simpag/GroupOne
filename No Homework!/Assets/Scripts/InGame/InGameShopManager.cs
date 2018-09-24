using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameShopManager : MonoBehaviour {

    private static InGameShopManager instance;
    public static InGameShopManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public List<InGameShopItem> shopItems;

    public enum TowerList
    {
        test
    }

    private void Awake()
    {
        //Create singleton
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

    public static void PurchasedTower (InGameShopItem _bought)
    {
        AccountInfo.Instance.InGameCurrency -= _bought.Cost;
    }

    public void StartTowerSelectionButton(int _index)
    {
        switch (_index)
        {
            case (int)TowerList.test:
                StartTowerSelection(shopItems[0]);
                break;
        }
    }

    public void StartTowerSelection(InGameShopItem _tower)
    {
        BuildManager.SelectTower(_tower);
    }

    private void SetupShopItems()
    {
        //Gamesparks lateron
    }
}
