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
    [SerializeField]
    private GameObject locked;
    private bool owned;

    private void Start()
    {
        locked.SetActive(false);
    }

    public void BuyItem ()
    {
        if (!owned)
            StoreManager.BuyItem(Stats);
    }

    public void UpdateItem(bool _own)
    {
        owned = _own;

        if (_own)
        {
            costText.text = "Owned";
        }
        else
        {
            costText.text = Stats.Cost.ToString();
            locked.SetActive(true);
        }

        if (AccountInfo.Instance.Currency >= stats.Cost)
            locked.SetActive(false);
    }
}
