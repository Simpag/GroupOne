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

    public void ShowToolTip(string _toolTipText, Vector2 _position)
    {
        Debug.Log("Showed: " + _toolTipText);

        instance.gameObject.SetActive(true);
        instance.toolTipText.text = _toolTipText;
        instance.rectTransform.anchoredPosition = _position;
    }

    public void HideToolTip()
    {
        Debug.Log("Hid tooltip");

        instance.gameObject.SetActive(false);
    }
}
