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
        foreach (StoreItem storeItem in DatabaseManager.Instance.StoreItems)
        {
            GameObject _sItem = Instantiate(Instance.storeItemPrefab, Instance.storeContent);
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
}
