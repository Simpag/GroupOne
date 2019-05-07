using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [Header("In-Game Info")]
    [SerializeField]
    private int homework;
    [SerializeField]
    private float candyCurrency;
    [SerializeField]
    private float startingCash = 500f;
    [SerializeField]
    private float cashSpent;

    public float CashSpent
    {
        get { return cashSpent; }
    }


    public static int Homework
    {
        get { return Instance.homework; }
    }
    public static float CandyCurrency
    {
        get { return Instance.candyCurrency; }
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

        Instance.candyCurrency = startingCash;

        AudioManager.Instance.Play("InGameMusic");
    }

    public static void RemoveCandyCurrency(float _amount)
    {
        Instance.candyCurrency -= _amount;

        if (Instance.candyCurrency < 0)
        {
            Instance.candyCurrency = 0;
        }

        Instance.cashSpent += _amount;
        InGameUIManager.UpdateCandyText();
    }

    public static void AddCandyCurrency(float _amount)
    {
        Instance.candyCurrency += _amount;
        InGameUIManager.UpdateCandyText();
    }

    public static void AddHomework(int _amount)
    {
        Instance.homework += _amount;
        InGameUIManager.UpdateHomework();

        if (Instance.homework >= HomeworkBar.Instance.maxHomework)
        {
            GameManager.EndGame(false);
        }
    }

    public static void SetHomework(int _amount)
    {
        Instance.homework = _amount;
        InGameUIManager.UpdateHomework();

        if (Instance.homework >= HomeworkBar.Instance.maxHomework)
        {
            GameManager.EndGame(false);
        }
    }
}
