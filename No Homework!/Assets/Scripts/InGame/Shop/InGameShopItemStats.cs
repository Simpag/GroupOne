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
    private float baseCost;
    [SerializeField]
    private float[] upgradeRow1Cost;
    [SerializeField]
    private float[] upgradeRow2Cost;

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
        get { towerPrefab.GetComponent<StudentStats>().shopStats = this; return towerPrefab; }
    }
    public float BaseCost
    {
        get { return baseCost; }
    }
    public float[] UpgradeRow1Cost
    {
        get { return upgradeRow1Cost; }
    }
    public float[] UpgradeRow2Cost
    {
        get { return upgradeRow2Cost; }
    }

    //public void Setup (InGameShopItemStats _stat)
    //{
    //    this.tower = _stat.Tower;
    //    this.prefab = _stat.Prefab;
    //    this.cost = _stat.Cost;
    //}
}
