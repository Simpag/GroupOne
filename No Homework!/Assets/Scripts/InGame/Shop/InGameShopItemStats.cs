using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InGameShopItemStats {

    [SerializeField]
    private InGameShopManager.TowerList tower;
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private GameObject shopPrefab;
    [SerializeField]
    private float cost;

    public InGameShopManager.TowerList Tower
    {
        get { return tower; }
        set { tower = value; }
    }
    public GameObject TowerPrefab
    {
        get { return towerPrefab; }
    }
    public GameObject ShopPrefab
    {
        get { return shopPrefab; }
    }
    public float Cost
    {
        get { return cost; }
    }

    //public void Setup (InGameShopItemStats _stat)
    //{
    //    this.tower = _stat.Tower;
    //    this.prefab = _stat.Prefab;
    //    this.cost = _stat.Cost;
    //}
}
