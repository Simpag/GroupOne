using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour {
    private static InGameUIManager instance;

    [Header("Drag-n-Drop")]
    [SerializeField]
    private Text homeworkText;
    [SerializeField]
    private Text candyText;

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
    }

    public static void UpdateHomeworkText ()
    {
        instance.homeworkText.text = PlayerStats.Homework.ToString();
    }

    public static void UpdateCandyText ()
    {
        instance.candyText.text = Mathf.RoundToInt(PlayerStats.CandyCurrency).ToString();
    }
}
