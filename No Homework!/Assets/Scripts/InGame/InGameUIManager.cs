using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour {
    private static InGameUIManager instance;

    [Header("Banner")]
    [SerializeField]
    private GameObject banner;
    [SerializeField]
    private Text homeworkText;
    [SerializeField]
    private Text candyText;

    [Header("Shop")]
    [SerializeField]
    private GameObject shopView;
    [SerializeField]
    private GameObject showShotButton;

    [Header("Tower Information")]
    [SerializeField]
    private GameObject towerInformationView;
    [SerializeField]
    private Text towerInformationName;
    [SerializeField]
    private Text towerInformationDescription;
    private Tower towerInfo;

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
    }

    private void Start()
    {
        UpdateHomeworkText();
        UpdateCandyText();
        showShotButton.SetActive(true);
        shopView.SetActive(false);
        towerInformationView.SetActive(false);
        banner.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !BuildManager.Instance.TowerIsSelected)
        {
            if (shopView.activeSelf)
                ShowOrHideShop();

            if (towerInformationView.activeSelf)
                HideTowerInfo();
        }
    }

    public static void UpdateHomeworkText ()
    {
        instance.homeworkText.text = PlayerStats.Homework.ToString();
    }

    public static void UpdateCandyText ()
    {
        instance.candyText.text = Mathf.RoundToInt(PlayerStats.CandyCurrency).ToString();
    }

    public void ShowOrHideShop()
    {
        if (shopView.activeSelf) //Hide
        {
            shopView.SetActive(false);
            showShotButton.SetActive(true);
        }
        else //Show
        {
            showShotButton.SetActive(false);
            shopView.SetActive(true);

            HideTowerInfo();
        }
    }

    public void UpgradeSelectedTower()
    {
        InGameShopManager.UpgradeTower(towerInfo);
    }

    public static void ShowTowerInfo (Tower _tower)
    {
        instance.towerInfo = _tower; //Save the selected Tower

        if (instance.shopView.activeSelf)
            instance.ShowOrHideShop();

        instance.towerInformationView.SetActive(true);
        instance.towerInformationName.text = _tower.towerName;
        instance.towerInformationDescription.text = _tower.towerDescription;
    }

    private void HideTowerInfo()
    {
        instance.towerInformationView.SetActive(false);
        towerInfo = null;
    }
}
