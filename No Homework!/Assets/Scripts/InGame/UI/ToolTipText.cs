using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipText : MonoBehaviour
{
    [TextArea]
    [SerializeField]
    private string text;

    private ToolTip toolTip;

    private void Awake()
    {
        toolTip = gameObject.AddComponent<ToolTip>();
        toolTip.text = text;
    }
}

public class ToolTip : EventTrigger {

    public string text;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("Enter");
        ToolTipManager.Instance.ShowToolTip(text);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        Debug.Log("Exit");
        ToolTipManager.Instance.HideToolTip();   
    }
}
