using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeworkBar : MonoBehaviour {

    private static HomeworkBar instance;
    public static HomeworkBar Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public Image homeworkBar;

    public int add = 0;

    private float maxHomework = 250f;

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

        UpdateHomeworkBar();
    }

    private void Update()
    {
        if (add != 0)
        {
            PlayerStats.AddHomework(add);
            add = 0;
        }
    }

    // Update is called once per frame
    public static void UpdateHomeworkBar()
    {
        Instance.homeworkBar.fillAmount = PlayerStats.Homework / Instance.maxHomework;
    }
}
