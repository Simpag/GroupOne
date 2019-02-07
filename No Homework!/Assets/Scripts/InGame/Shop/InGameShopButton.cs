using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameShopButton : EventTrigger {

    private Image buttonImage, studentIcon;
    public InGameShopItemStats stat;
    private Text cost;

    private void Start()
    {
        cost = GetComponentInChildren<Text>();
        buttonImage = GetComponent<Image>();
        studentIcon = transform.Find("StudentIcon").GetComponent<Image>();

        cost.text = stat.BaseCost.ToString();
        studentIcon.sprite = stat.StudentIcon;
    }

    public override void OnPointerDown(PointerEventData data)
    {
        SelectTower(stat.Tower);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        HighliteButton(true);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        HighliteButton(false);
    }

    private void SelectTower(InGameShopManager.TowerList _tower)
    {
        InGameShopManager.StartTowerSelection(stat);
    }

    private void HighliteButton(bool highlite)
    {
        if (highlite)
            buttonImage.CrossFadeAlpha(0.2f, 0.2f, false);
        else
            buttonImage.CrossFadeAlpha(1f, 0.2f, false);
    }
}
