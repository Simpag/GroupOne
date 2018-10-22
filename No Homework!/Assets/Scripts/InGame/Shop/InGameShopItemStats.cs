using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InGameShopItemStats {

    [SerializeField]
    private string shortCode;
    [SerializeField]
    private InGameShopManager.TowerList tower;
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private float cost;
    [SerializeField]
    private float upgradeCost;

    public string ShortCode
    {
        get { return shortCode; }
        set { shortCode = value; }
    }
    public InGameShopManager.TowerList Tower
    {
        get { return tower; }
        set { tower = value; }
    }
    public GameObject TowerPrefab
    {
        //Set the shopstats then return the prefab with the stats
        get { towerPrefab.GetComponent<Tower>().shopStats = this; return towerPrefab; }
    }
    public float Cost
    {
        get { return cost; }
    }
    public float UpgradeCost
    {
        get { return upgradeCost; }
    }

    //public void Setup (InGameShopItemStats _stat)
    //{
    //    this.tower = _stat.Tower;
    //    this.prefab = _stat.Prefab;
    //    this.cost = _stat.Cost;
    //}
}
