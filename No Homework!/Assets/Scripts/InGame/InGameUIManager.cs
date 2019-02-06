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

    [Header("Student Information")]
    [SerializeField]
    private GameObject studentInformationView;
    [SerializeField]
    private Text studentInformationName;
    [SerializeField]
    private Text studentInformationDescription;
    [SerializeField]
    private Dropdown studentTargetPriorityDropdown;
    [SerializeField]
    private Text studentUpgradeCost;
    private StudentStats currentStudentStats;
    private List<StudentStats.TargetSetting> currentlyAllowedTargetSettings;

    private float shopTimer = 0;

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
        studentInformationView.SetActive(false);
        banner.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !BuildManager.Instance.TowerIsSelected)
        {
            if (shopView.activeSelf)
                ShowOrHideShop();

            if (studentInformationView.activeSelf)
                HideTowerInfo();
        }

        if(shopTimer > 0)
        {
            shopTimer -= Time.deltaTime;
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
        if (shopTimer > Mathf.Epsilon)
            return;
            
        if (shopView.activeSelf) //Hide
        {
            shopAnim.SetTrigger("Hide");
        }
        else //Show
        {
            shopAnim.SetTrigger("Show");

            HideTowerInfo();
        }

        shopTimer = 0.5f;
    }

    public void UpgradeSelectedStudentRow1()
    {
        BuildManager.Instance.UpgradeStudentRow1(currentStudentStats);
    }

    public void UpgradeSelectedStudentRow2()
    {
        BuildManager.Instance.UpgradeStudentRow2(currentStudentStats);
    }

    public static void ShowTowerInfo (StudentStats _tower)
    {
        instance.currentStudentStats = _tower; //Save the selected Tower

        instance.studentInformationView.SetActive(true); //Active the windows
        instance.studentInformationName.text = _tower.studentName;  //Set the name of the tower
        instance.studentInformationDescription.text = _tower.studentDescription;    //Set the description
        instance.studentUpgradeCost.text = _tower.shopStats.UpgradeRow1Cost[0].ToString();   //Set the upgrade cost

        instance.SetupTowerTargetPriority(_tower);

        instance.currentStudentStats.rangeView.GetComponent<MeshRenderer>().enabled = true; //Show the range of the tower
    }

    private void SetupTowerTargetPriority(StudentStats _tower)
    {
        studentTargetPriorityDropdown.ClearOptions();
        List<Dropdown.OptionData> _allowedTargetSettings = new List<Dropdown.OptionData>();
        currentlyAllowedTargetSettings = new List<StudentStats.TargetSetting>();

        foreach (StudentStats.TargetSetting _ts in _tower.allowedTargetSettings)
        {
            string _tsString = "";
            switch(_ts)
            {
                case StudentStats.TargetSetting.first:
                    _tsString = "First";
                    break;
                case StudentStats.TargetSetting.last:
                    _tsString = "Last";
                    break;
                case StudentStats.TargetSetting.leastHealth:
                    _tsString = "Least Health";
                    break;
                case StudentStats.TargetSetting.mostHealth:
                    _tsString = "Most Health";
                    break;
            }

            _allowedTargetSettings.Add(new Dropdown.OptionData(_tsString));
            currentlyAllowedTargetSettings.Add(_ts);
        }

        studentTargetPriorityDropdown.AddOptions(_allowedTargetSettings);

        studentTargetPriorityDropdown.value = currentlyAllowedTargetSettings.IndexOf(_tower.currentTargetSetting);
    }

    private void HideTowerInfo()
    {
        instance.studentInformationView.SetActive(false);

        if (instance.currentStudentStats == null)
            return;

        instance.currentStudentStats.rangeView.GetComponent<MeshRenderer>().enabled = false; //Show the range of the tower
        instance.currentStudentStats = null;
    }

    public void ChangedTowerTargetPriority()
    {
        currentStudentStats.currentTargetSetting = currentlyAllowedTargetSettings[studentTargetPriorityDropdown.value];
    }
}
