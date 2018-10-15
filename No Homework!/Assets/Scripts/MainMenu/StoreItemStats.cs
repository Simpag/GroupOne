using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Responses;

[System.Serializable]
public class StoreItemStats {

    [SerializeField]
    private ListVirtualGoodsResponse._VirtualGood item;
    public ListVirtualGoodsResponse._VirtualGood Item
    {
        get { return item; }
        set { item = value; }
    }

    [SerializeField]
    private StoreItemManager manager;
    public StoreItemManager Manager
    {
        get { return manager; }
        set { manager = value; }
    }


    [SerializeField]
    public string Name
    {
        get
        {
            return item.Name;
        }
    }

    [SerializeField]
    public string ShortCode
    {
        get
        {
            return item.ShortCode;
        }
    }

    [SerializeField]
    public float Cost
    {
        get
        {
            Debug.Log((float)item.CurrencyCosts.GetFloat(GameConstants.CURRENCY_CANDY_COIN));
            return (float)item.CurrencyCosts.GetFloat(GameConstants.CURRENCY_CANDY_COIN);
        }
    }
}
