using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashButton : EventTrigger {

    [SerializeField]
    private static TrashButton instance;
    public static TrashButton Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private Image trashImage;
    private bool trash = false;
    public static bool Cancel { get { return instance.trash; } }

    private void Start()
    {
        trashImage = GetComponent<Image>();

        //Create singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        trash = false;
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        trash = true;
        HighliteButton(true);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        trash = false;
        HighliteButton(false);
    }

    private void HighliteButton(bool highlite)
    {
        if (highlite)
        {
            trashImage.CrossFadeAlpha(0.2f, 0.2f, false);
        }
        else
        {
            trashImage.CrossFadeAlpha(1f, 0.2f, false);
        }
    }
}
