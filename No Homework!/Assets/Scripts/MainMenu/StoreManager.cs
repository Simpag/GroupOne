using GameSparks.Api.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour {

    private static StoreManager instance;
    public static StoreManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [SerializeField]
    private Transform storeContent;
    [SerializeField]
    private GameObject storeItemPrefab;

    private AccountInfo player = AccountInfo.Instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static void ShowStore()
    {
        foreach (StoreItemStats storeItemStats in DatabaseManager.Instance.StoreItemStats)
        {
            GameObject _sItem = Instantiate(Instance.storeItemPrefab, Instance.storeContent);
            StoreItemManager _sItemMan = _sItem.GetComponent<StoreItemManager>();
            _sItemMan.Stats = storeItemStats;
            _sItemMan.UpdateItem();
        }
    }

    public static void HideStore()
    {
        if (Instance.storeContent.childCount <= 0)
            return;

        for (int i = 0; i < Instance.storeContent.childCount; i++)
        {
            Destroy(Instance.storeContent.GetChild(i));
        }
    }

    public static void BuyItem(StoreItemStats _stats)
    {
        if (Instance.player.Currency >= _stats.Cost)
        {
            new BuyVirtualGoodsRequest()
            .SetCurrencyShortCode(GameConstants.CURRENCY_CANDY_COIN)
            .SetQuantity(1)
            .SetShortCode(_stats.ShortCode)
            .Send((response) => {
                if (response.HasErrors)
                    Debug.LogError("Couldnt buy item: " + response.Errors.JSON);
                else
                    Instance.BoughtItemSuccessfully(_stats);
            });
        }
    }

    private void BoughtItemSuccessfully(StoreItemStats _stats)
    {
        Debug.Log("Successfully bought" + _stats.Name);

        _stats.Manager.UpdateItem();
        AccountInfo.UpdateAccountInfo();
    }
}
