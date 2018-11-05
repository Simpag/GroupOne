using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System.Linq;
using System.IO;

public class DatabaseManager : MonoBehaviour {

    private static DatabaseManager instance;
    public static DatabaseManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private GSEnumerable<ListVirtualGoodsResponse._VirtualGood> virtualGoods;
    public GSEnumerable<ListVirtualGoodsResponse._VirtualGood> VirtualGoods
    {
        get { return virtualGoods; }
        set { virtualGoods = value; }
    }

    [SerializeField]
    private List<StoreItemStats> storeItemStats;
    public List<StoreItemStats> StoreItemStats
    {
        get { return storeItemStats; }
        set { storeItemStats = value; }
    }

    //Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public static void UpdateDatabase()
    {
        new ListVirtualGoodsRequest()
        .SetIncludeDisabled(false)
        .Send((response) => {
            Instance.VirtualGoods = response.VirtualGoods;
            OnUpdateDatabase();
        });
    }

    private static void OnUpdateDatabase()
    {
        Debug.Log("Updated Database");
        if (Instance.VirtualGoods == null)
            return;

        foreach (ListVirtualGoodsResponse._VirtualGood _vgood in Instance.VirtualGoods)
        {
            if (_vgood.Tags.Contains(GameConstants.ITEM_STORE))
            {
                Instance.StoreItemStats.Add(CreateStoreItem(_vgood));   //Save list of store items
            }
        }
    }

    private static StoreItemStats CreateStoreItem(ListVirtualGoodsResponse._VirtualGood _item)
    {
        StoreItemStats _sItem = new StoreItemStats();

        _sItem.Item = _item;

        return _sItem;
    }
}
