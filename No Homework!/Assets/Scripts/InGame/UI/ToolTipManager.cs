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

    public void Start()
    {
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform, Input.mousePosition,
            parentCanvas.worldCamera,
            out pos);
    }

    private void Update()
    {
        if (!toolTipText.gameObject.activeSelf)
            return;

        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        if ((Screen.width - (movePos.x + offset.x * 2)) > 0)
        {
            //Inside screen space
            transform.position = parentCanvas.transform.TransformPoint(movePos);
        }
        else
        {
            //Outside of screen space
            transform.position = parentCanvas.transform.TransformPoint(new Vector2(Screen.width - offset.x * 4, movePos.y));
        }
    }

    public void ShowToolTip(string _toolTipText)
    {
        toolTipText.gameObject.SetActive(true);
        instance.toolTipText.text = _toolTipText;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            (toolTipRect.rect.size), parentCanvas.worldCamera,
            out offset);
    }

    public void HideToolTip()
    {
        toolTipText.gameObject.SetActive(false);
    }
}
