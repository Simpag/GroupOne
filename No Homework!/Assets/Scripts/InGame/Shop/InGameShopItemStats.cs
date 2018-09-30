using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InGameShopItemStats {

    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private float cost;

    public GameObject Prefab
    {
        get { return prefab; }
    }
    public float Cost
    {
        get { return cost; }
    }


}
