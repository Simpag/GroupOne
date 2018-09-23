using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InGameShopItem {

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
