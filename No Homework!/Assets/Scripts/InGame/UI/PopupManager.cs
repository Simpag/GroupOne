using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour {

    private static PopupManager instance;
    public static PopupManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [SerializeField]
    private string[] popuptext;
    [SerializeField]
    private GameObject popup;

    private int index = 0;

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

        popup.SetActive(false);

        Invoke("ShowNextPopup", 5f);
        Invoke("ShowNextPopup", 10f);
        Invoke("ShowNextPopup", 20f);
        Invoke("ShowNextPopup", 60f);
    }

    public void ShowNextPopup()
    {
        if (Instance.index >= Instance.popuptext.Length)
            return;

        Instance.popup.SetActive(true);
        Instance.popup.GetComponent<Popup>().Setup(Instance.popuptext[Instance.index]);
        Instance.index++;
    }

}
