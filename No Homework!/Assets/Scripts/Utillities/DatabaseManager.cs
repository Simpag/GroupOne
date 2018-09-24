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

    }

    private static void OnUpdateDatabase()
    {

    }
}
