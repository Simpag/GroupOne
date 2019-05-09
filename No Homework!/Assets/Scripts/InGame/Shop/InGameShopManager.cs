using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameShopManager : MonoBehaviour {

    private static InGameShopManager instance;
    public static InGameShopManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [Header("Drag-n-Drop")]
    [SerializeField]
    private GameObject ShopPrefab;
    [SerializeField]
    private Transform shopView;

    [Header("Fill in the list with towers!")]
    public List<InGameShopItemStats> allShopItems;

    [Header("Don't touched, unlocked during game!")]
    [SerializeField]
    private List<InGameShopItemStats> unlockedShopItems;

    public enum TowerList
    {
        ThrowingStudent,
        SpitStudent,
        SlingshotStudent,
        NerdStudent,
        FootballStudent,
        PaperplaneStudent
    }

    private void Awake()
    {
        //Create singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetupShopItems();
    }

    public static void PurchasedStudent(InGameShopItemStats _bought)
    {
        PlayerStats.RemoveCandyCurrency(_bought.BaseCost);
    }

    public static void SoldStudent(InGameShopItemStats _sold)
    {
        PlayerStats.AddCandyCurrency(_sold.SellWorth);
    }

    public static void StartTowerSelection(InGameShopItemStats _tower)
    {
        if (PlayerStats.CandyCurrency >= _tower.BaseCost)
            BuildManager.SelectTowerToBuild(_tower);
        else
        {
            Debug.Log("not enought money");
        }
    }

    public static bool UpgradeStudent(StudentStats _info, int _row)
    {
        if (_info == null || !Instance.CanUpgradeRow(_info, _row) || _row > 2 || _row < 1)
            return false;

        float _currentRowUpgradeCost = float.MaxValue;

        switch (_row)
        {
            case 1:
                _currentRowUpgradeCost = _info.shopStats.UpgradeRow1Cost[_info.Row1Level];
                break;
            case 2:
                _currentRowUpgradeCost = _info.shopStats.UpgradeRow2Cost[_info.Row2Level];
                break;
        }

        if (PlayerStats.CandyCurrency >= _currentRowUpgradeCost)
        {
            switch (_row)
            {
                case 1:
                    _info.shopStats.SellWorth += _info.shopStats.UpgradeRow1SellWorth[_info.Row1Level];
                    _info.UpgradeRow1();
                    break;
                case 2:
                    _info.shopStats.SellWorth += _info.shopStats.UpgradeRow2SellWorth[_info.Row2Level];
                    _info.UpgradeRow2();
                    break;
            }
            PlayerStats.RemoveCandyCurrency(_currentRowUpgradeCost); // Fix this
            return true;
        }
        else
        {
            Debug.Log("Not enought money!");
            return false;
        }
    }

    private bool CanUpgradeRow(StudentStats _info, int _row)
    {
        switch (_row)
        {
            case 1:
                if ((_info.Row2Level <= 1 || _info.Row1Level < 1) && _info.Row1Level < GameConstants.NUMBER_OF_UPGRADES)
                {
                    return true;
                }
                break;
            case 2:
                if ((_info.Row1Level <= 1 || _info.Row2Level < 1) && _info.Row2Level < GameConstants.NUMBER_OF_UPGRADES)
                {
                    return true;
                }
                break;
        }

        Debug.LogError("Could not upgrade row: " + _row);
        return false;
    }

    private void SetupShopItems()
    {
        if (!GameManager.IsOffline)
        {
            //Unlock towers
            for (int i = 0; i < AccountInfo.Instance.Inventory.BaseData.Keys.Count; i++)
            {
                foreach (InGameShopItemStats _igStats in allShopItems)
                {
                    if (AccountInfo.Instance.Inventory.ContainsKey(_igStats.ShortCode))
                    {
                        if (!unlockedShopItems.Contains(_igStats))
                            unlockedShopItems.Add(_igStats);
                    }
                }
            }
        }
        else //If its offline mode
        {
            foreach (InGameShopItemStats _igStats in allShopItems)
            {
                unlockedShopItems.Add(_igStats);
            }
        }

        //Instantiate towers to shop
        foreach (InGameShopItemStats _stats in unlockedShopItems)
        {
            GameObject _shopGo = Instantiate(ShopPrefab, shopView);

            _shopGo.GetComponent<InGameShopButton>().stat = _stats;
        }
    }
}
