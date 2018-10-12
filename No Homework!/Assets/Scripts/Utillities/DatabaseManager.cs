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
    private List<StoreItem> storeItems;
    public List<StoreItem> StoreItems
    {
        get { return storeItems; }
        set { storeItems = value; }
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
        foreach (ListVirtualGoodsResponse._VirtualGood _vgood in Instance.VirtualGoods)
        {
            if (_vgood.Tags.Contains(GameConstants.ITEM_STORE))
            {
                Instance.StoreItems.Add(CreateStoreItem(_vgood));   //Save list of store items
                Debug.Log("Added Item" + _vgood.Name);
            }
        }
    }

    private static StoreItem CreateStoreItem(ListVirtualGoodsResponse._VirtualGood _item)
    {
        StoreItem _sItem = new StoreItem();

        _sItem.Item = _item;

        return _sItem;
    }
}
