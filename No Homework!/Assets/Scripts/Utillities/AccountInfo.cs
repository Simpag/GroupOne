using UnityEngine;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System.Collections.Generic;
using System.Collections;
using System;

public class AccountInfo : MonoBehaviour {

    private static AccountInfo instance;
    public static AccountInfo Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [Header("Account Info")]
    [SerializeField]
    private float? currency;
    [SerializeField]
    private string displayName;
    [SerializeField]
    private string userId;
    [SerializeField]
    private GSData virtualGoods;

    public GSData Inventory
    {
        get { return virtualGoods; }
        set { virtualGoods = value; }
    }
    public string UserId
    {
        get { return userId; }
        set { userId = value; }
    }
    public string DisplayName
    {
        get { return displayName; }
        set { displayName = value; }
    }
    public float? Currency
    {
        get { return currency; }
        set { currency = value; }
    }

    [Header("In-Game Info")]
    [SerializeField]
    private float inGameCurrency;

    public float InGameCurrency
    {
        get { return inGameCurrency; }
        set { inGameCurrency = value; }
    }


    //Create singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void UpdateAccountInfo()
    {
        new AccountDetailsRequest()
        .Send((response) =>
        {
            if (response.HasErrors)
            {
                Debug.LogError(response.Errors);
                return;
            }

            Instance.UserId = response.UserId;
            Instance.DisplayName = response.DisplayName;
            Instance.Currency = 0f; //response.Currencies.GetFloat(GameConstants.CURRENCY_CODE);
            Instance.Inventory = response.VirtualGoods;
        });
    }
}
