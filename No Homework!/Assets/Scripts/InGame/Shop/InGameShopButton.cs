﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameShopButton : EventTrigger {

    private Image buttonImage;
    public InGameShopItemStats stats;
    private Text cost;

    private void Awake()
    {
        cost = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        cost.text = stats.BaseCost.ToString();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        SelectTower(stats.Tower);
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
        InGameShopManager.StartTowerSelection(InGameShopManager.Instance.allShopItems[(int)_tower]);
    }

    private void HighliteButton(bool highlite)
    {
        if (highlite)
            buttonImage.CrossFadeAlpha(0.2f, 0.2f, false);
        else
            buttonImage.CrossFadeAlpha(1f, 0.2f, false);
    }
}
