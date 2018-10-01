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

    [SerializeField]
    private List<InGameShopItemStats> shopItems;
    public static List<InGameShopItemStats> ShopItems
    {
        get { return Instance.shopItems; }
    }


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
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public static void PurchasedTower (InGameShopItemStats _bought)
    {
        PlayerStats.RemoveCandyCurrency(_bought.Cost);
    }

    public static void StartTowerSelection(InGameShopItemStats _tower)
    {
        if (PlayerStats.CandyCurrency >= _tower.Cost)
            BuildManager.SelectTowerToBuild(_tower);
        else
        {
            //not enought money
        }
    }

    private void SetupShopItems()
    {
        //Gamesparks lateron
    }
}
