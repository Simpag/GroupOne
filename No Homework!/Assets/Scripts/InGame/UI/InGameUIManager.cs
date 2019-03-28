﻿using System.Collections;
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

    [Header("On Screen")]
    [SerializeField]
    private GameObject nextWave;
    [SerializeField]
    private Text waveIndex;

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
    private StudentInfoUpgradeButton[] row1UpgradeButtons;
    [SerializeField]
    private StudentInfoUpgradeButton[] row2UpgradeButtons;
    private StudentStats currentStudentStats;
    private List<StudentStats.TargetSetting> currentlyAllowedTargetSettings;

    [Header("Pause Menu")]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private GameObject singleplayerBackground;
    [SerializeField]
    private GameObject multiplayerBackground;

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
        nextWave.SetActive(true);
        pauseMenu.SetActive(false);
        multiplayerBackground.SetActive(false);
        singleplayerBackground.SetActive(false);

        if (GameManager.IsMultiplayer)
            multiplayerBackground.SetActive(true);
        else
            singleplayerBackground.SetActive(true);
    }

    private void Update()
    {
        /*Very bunk, change */
        if (studentInformationView.activeSelf == false && shopView.activeSelf == false)
            nextWave.SetActive(true);
        else
            nextWave.SetActive(false);

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !BuildManager.Instance.TowerIsSelected)
        {
            if (shopView.activeSelf)
                ShowOrHideShop();

            if (studentInformationView.activeSelf)
                HideTowerInfo();
        }

        if (currentStudentStats != null)
        {
            if (currentStudentStats.Row2Level <= 1 && currentStudentStats.Row1Level < GameConstants.NUMBER_OF_UPGRADES && PlayerStats.CandyCurrency >= currentStudentStats.shopStats.UpgradeRow1Cost[currentStudentStats.Row1Level])
            {
                row1UpgradeButtons[currentStudentStats.Row1Level].SetState(StudentInfoUpgradeButton.ButtonState.available);
            }
            else if (currentStudentStats.Row2Level <= 1 && currentStudentStats.Row1Level < GameConstants.NUMBER_OF_UPGRADES)
            {
                row1UpgradeButtons[currentStudentStats.Row1Level].SetState(StudentInfoUpgradeButton.ButtonState.locked);
            }

            if (currentStudentStats.Row1Level <= 1 && currentStudentStats.Row2Level < GameConstants.NUMBER_OF_UPGRADES && PlayerStats.CandyCurrency >= currentStudentStats.shopStats.UpgradeRow2Cost[currentStudentStats.Row2Level])
            {
                row2UpgradeButtons[currentStudentStats.Row2Level].SetState(StudentInfoUpgradeButton.ButtonState.available);
            }
            else if (currentStudentStats.Row1Level <= 1 && currentStudentStats.Row2Level < GameConstants.NUMBER_OF_UPGRADES)
            {
                row2UpgradeButtons[currentStudentStats.Row2Level].SetState(StudentInfoUpgradeButton.ButtonState.locked);
            }
        }

        if (shopTimer > 0)
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

    public static void UpdateWaveIndex()
    {
        instance.waveIndex.text = "Wave: " + WaveSpawner.Instance.WaveIndex.ToString();
    }

    public void ShowOrHideShop()
    {
        if (GameFunctions.IsGamePaused)
            return;

        if (shopTimer > Mathf.Epsilon)
            return;
            
        if (shopView.activeSelf) //Hide
        {
            shopAnim.SetTrigger("Hide");
            pauseButton.SetActive(true);
        }
        else //Show
        {
            shopAnim.SetTrigger("Show");

            HideTowerInfo();
            pauseButton.SetActive(false);
        }

        shopTimer = 0.5f;
    }

    public void SellSelectedStudent()
    {
        BuildManager.Instance.SellStudent(currentStudentStats, true);
        HideTowerInfo();
    }

    public void UpgradeSelectedStudentRow1()
    {
        if (BuildManager.Instance.UpgradeStudentRow1(currentStudentStats))
            row1UpgradeButtons[currentStudentStats.Row1Level - 1].SetState(StudentInfoUpgradeButton.ButtonState.bought);
        else
            row1UpgradeButtons[currentStudentStats.Row1Level].SetState(StudentInfoUpgradeButton.ButtonState.locked);
    }

    public void UpgradeSelectedStudentRow2()
    {
        if (BuildManager.Instance.UpgradeStudentRow2(currentStudentStats)) //If completed
            row2UpgradeButtons[currentStudentStats.Row2Level - 1].SetState(StudentInfoUpgradeButton.ButtonState.bought);
        else
            row2UpgradeButtons[currentStudentStats.Row2Level].SetState(StudentInfoUpgradeButton.ButtonState.locked);
    }

    public static void ShowTowerInfo (StudentStats _tower)
    {
        if (GameFunctions.IsGamePaused)
            return;

        instance.currentStudentStats = _tower; //Save the selected Tower

        instance.studentInformationView.SetActive(true); //Active the windows
        instance.studentInformationName.text = _tower.studentName;  //Set the name of the tower
        instance.studentInformationDescription.text = _tower.studentDescription;    //Set the description
        instance.SetupTowerTargetPriority(_tower);

        for (int i = 0; i < _tower.Row1Level; i++)
        {
            instance.row1UpgradeButtons[i].SetState(StudentInfoUpgradeButton.ButtonState.bought);
        }

        for (int i = 0; i < _tower.Row2Level; i++)
        {
            instance.row2UpgradeButtons[i].SetState(StudentInfoUpgradeButton.ButtonState.bought);

        }

        //Show image
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
        if (GameFunctions.IsGamePaused)
            return;

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

    public void PauseOrResumeGame()
    {
        if (!GameFunctions.IsGamePaused)
        {
            GameFunctions.PauseGame();
            pauseMenu.SetActive(true);
        }
        else
        {
            GameFunctions.ResumeGame();
            pauseMenu.SetActive(false);
        }
    }

    public void RestartGame()
    {
        GameFunctions.ResumeGame();
        GameManager.StartGame(GameManager.Startmethod.singleplayer);
    }

    public void ReturnToMainMenu()
    {
        GameManager.EndGame(false);
    }

    public void Disconnect()
    {
        MultiplayerManager.Disconnect();
    }

    public void MuteOrUnmuteMusic()
    {
        AudioManager.Instance.MuteOrUnmuteMusic();
    }

    public void MuteOrUnmuteSoundEffects()
    {
        AudioManager.Instance.MuteOrUnmuteSoundEffects();
    }
}

