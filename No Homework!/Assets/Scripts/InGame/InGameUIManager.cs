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
    [SerializeField]
    private Text towerUpgradeCost;
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
        UpdateHomework();
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

    public static void UpdateHomework ()
    {
        HomeworkBar.UpdateHomeworkBar();
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
        BuildManager.Instance.UpgradeTower(towerInfo);
    }

    public static void ShowTowerInfo (Tower _tower)
    {
        instance.towerInfo = _tower; //Save the selected Tower

        if (instance.shopView.activeSelf)
            instance.ShowOrHideShop();

        instance.towerInformationView.SetActive(true);
        instance.towerInformationName.text = _tower.towerName;
        instance.towerInformationDescription.text = _tower.towerDescription;
        instance.towerUpgradeCost.text = _tower.shopStats.UpgradeCost.ToString();

        instance.towerInfo.rangeView.GetComponent<MeshRenderer>().enabled = true; //Show the range of the tower
    }

    private void HideTowerInfo()
    {
        instance.towerInformationView.SetActive(false);

        if (instance.towerInfo == null)
            return;

        instance.towerInfo.rangeView.GetComponent<MeshRenderer>().enabled = false; //Show the range of the tower
        instance.towerInfo = null;
    }

    public void ChangeTowerTargetPriority()
    {
        if (towerInfo.targetSetting == Tower.TargetSetting.first)
        {
            towerInfo.targetSetting = Tower.TargetSetting.last;
            Debug.Log("Last");
        } else
        {
            towerInfo.targetSetting = Tower.TargetSetting.first;
            Debug.Log("First");
        }
    }
}
