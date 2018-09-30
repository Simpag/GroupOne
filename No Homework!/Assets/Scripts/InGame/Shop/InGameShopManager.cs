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

    public List<InGameShopItemStats> shopItems;

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

    public void StartTowerSelectionButton(int _index)
    {
        switch (_index)
        {
            case (int)TowerList.test:
                StartTowerSelection(shopItems[0]);
                break;
        }
    }

    public void StartTowerSelection(InGameShopItemStats _tower)
    {
        if (PlayerStats.CandyCurrency >= _tower.Cost)
            BuildManager.SelectTowerToBuild(_tower);
    }

    private void SetupShopItems()
    {
        //Gamesparks lateron
    }
}
