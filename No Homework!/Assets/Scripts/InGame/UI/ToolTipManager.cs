using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour
{
    private static ToolTipManager instance;
    public static ToolTipManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private Text toolTipText;
    [SerializeField]
    private Canvas parentCanvas;
    [SerializeField]
    private RectTransform toolTipRect;

    private Vector2 offset;

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

    //public void Start()
    //{
    //    Vector2 pos;

    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        parentCanvas.transform as RectTransform, Input.mousePosition,
    //        parentCanvas.worldCamera,
    //        out pos);
    //}

    private void Update()
    {
        if (!toolTipText.gameObject.activeSelf)
            return;

        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        if ((movePos.x + offset.x/2) >= Screen.width/2)
        { 
            //Outside of horizontal screen space
            movePos.x = Screen.width/2 - offset.x/2;
        }

        transform.position = parentCanvas.transform.TransformPoint(movePos);
    }

    public void ShowToolTip(string _toolTipText)
    {
        toolTipText.gameObject.SetActive(true);
        instance.toolTipText.text = _toolTipText;

        offset = toolTipRect.rect.size;
    }

    public void HideToolTip()
    {
        toolTipText.gameObject.SetActive(false);
    }
}
