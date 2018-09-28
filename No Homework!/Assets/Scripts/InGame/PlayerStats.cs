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
    private float inGameCash;
    [SerializeField]
    private float startingCash = 500f;

    public int Homework
    {
        get { return homework; }
        set { homework = value; }
    }
    public static float InGameCash
    {
        get { return Instance.inGameCash; }
        set { Instance.inGameCash = value; }
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
    }

    private void Start()
    {
        InGameCash = startingCash;
    }

    public static void RemoveCash(float _amount)
    {
        InGameCash -= _amount;
    }

}
