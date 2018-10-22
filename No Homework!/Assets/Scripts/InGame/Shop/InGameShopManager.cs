﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameShopManager : MonoBehaviour {

    private static InGameShopManager instance;
    public static InGameShopManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [Header("Drag-n-Drop")]
    [SerializeField]
    private GameObject ShopPrefab;

    [Header("Fill in the list with towers!")]
    public List<InGameShopItemStats> allShopItems;

    [Header("Don't touched, unlocked during game!")]
    [SerializeField]
    private List<InGameShopItemStats> unlockedShopItems;

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

        SetupShopItems();
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
            Debug.Log("not enought money");
        }
    }

    public static void UpgradeTower(Tower _info)
    {
        if (_info == null || _info.towerLevel >= _info.numberOfUpgrades)
            return;

        if (PlayerStats.CandyCurrency >= _info.shopStats.UpgradeCost)
        {
            _info.UpgradeTower();
            PlayerStats.RemoveCandyCurrency(_info.shopStats.Cost);
        }
        else
        {
            Debug.Log("Not enought money!");
        }
    }

    private void SetupShopItems()
    {
        //Unlock towers
        for (int i = 0; i < AccountInfo.Instance.Inventory.BaseData.Keys.Count; i++)
        {
            foreach (InGameShopItemStats _igStats in allShopItems)
            {
                if (AccountInfo.Instance.Inventory.ContainsKey(_igStats.ShortCode))
                {
                    unlockedShopItems.Add(_igStats);
                }
            }
        }

        //Instantiate towers to shop
        foreach (InGameShopItemStats _stats in unlockedShopItems)
        {
            GameObject _shopGo = Instantiate(ShopPrefab, this.transform);

            _shopGo.GetComponent<InGameShopButton>().stats = _stats;
        }
    }
}
