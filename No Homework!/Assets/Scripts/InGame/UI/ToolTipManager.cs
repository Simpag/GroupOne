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

    private RectTransform rectTransform;

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

        rectTransform = GetComponent<RectTransform>();
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

        transform.position = parentCanvas.transform.TransformPoint(movePos);
    }

    public void ShowToolTip(string _toolTipText)
    {
        toolTipText.gameObject.SetActive(true);
        instance.toolTipText.text = _toolTipText;
    }

    public void HideToolTip()
    {
        toolTipText.gameObject.SetActive(false);
    }
}
