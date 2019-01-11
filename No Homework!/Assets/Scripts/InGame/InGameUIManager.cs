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
    private Animator shopAnim;
    [SerializeField]
    private GameObject shopView;
    [SerializeField]
    private GameObject showShopButton;

    [Header("Tower Information")]
    [SerializeField]
    private GameObject towerInformationView;
    [SerializeField]
    private Text towerInformationName;
    [SerializeField]
    private Text towerInformationDescription;
    [SerializeField]
    private Dropdown towerTargetPriority;
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
        showShopButton.SetActive(true);
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
            shopAnim.SetTrigger("Hide");
        }
        else //Show
        {
            shopAnim.SetTrigger("Show");

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

        instance.towerInformationView.SetActive(true); //Active the windows
        instance.towerInformationName.text = _tower.towerName;  //Set the name of the tower
        instance.towerInformationDescription.text = _tower.towerDescription;    //Set the description
        instance.towerUpgradeCost.text = _tower.shopStats.UpgradeCost.ToString();   //Set the upgrade cost

        instance.SetupTowerTargetPriority(_tower);

        instance.towerInfo.rangeView.GetComponent<MeshRenderer>().enabled = true; //Show the range of the tower
    }

    private void SetupTowerTargetPriority(Tower _tower)
    {
        towerTargetPriority.ClearOptions();
        List<Dropdown.OptionData> _allowedTargetSettings = new List<Dropdown.OptionData>();

        foreach (Tower.TargetSetting _ts in _tower.allowedTargetSettings)
        {
            _allowedTargetSettings.Add(new Dropdown.OptionData(_ts.ToString()));
        }

        towerTargetPriority.AddOptions(_allowedTargetSettings);

        towerTargetPriority.value ----- //Använd detta och jämför med allowedTargetSettings. För att ändra tower target prio
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
        /*if (towerInfo.targetSetting == Tower.TargetSetting.first)
        {
            towerInfo.targetSetting = Tower.TargetSetting.last;
        }
        else
        {
            towerInfo.targetSetting = Tower.TargetSetting.first;
        }*/
    }
}
