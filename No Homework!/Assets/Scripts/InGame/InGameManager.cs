using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour {

    private static InGameManager instance;
    public static InGameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public int startingCash = 500;

    private void Awake()
    {
        //Create singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AccountInfo.Instance.InGameCurrency = startingCash;
    }
}
