using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemManager : MonoBehaviour {

	[SerializeField]
    private StoreItemStats stats;
    public StoreItemStats Stats
    {
        get { return stats; }
        set { stats = value; }
    }

    [SerializeField]
    private Text costText;

    public void BuyItem ()
    {
        StoreManager.BuyItem(Stats);
    }

    public void UpdateItem()
    {
        costText.text = Stats.Cost.ToString();
    }
}
