using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentInfoUpgradeButton : MonoBehaviour
{
    [SerializeField]
    private Image boughtImage, lockedImage, unavailableImage;
    [SerializeField]
    private ButtonState currentState;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public enum ButtonState
    {
        locked,
        bought,
        available,
        unavailable
    }

    private void OnEnable()
    {
        SetState(ButtonState.locked);
    }

    public void SetState(ButtonState _state)
    {
        currentState = _state;

        switch (_state)
        {
            case ButtonState.available:
                boughtImage.enabled = false;
                lockedImage.enabled = false;

                button.enabled = true;
                break;
            case ButtonState.bought:
                boughtImage.enabled = true;
                lockedImage.enabled = false;

                button.enabled = false;
                break;
            case ButtonState.locked:
                boughtImage.enabled = false;
                lockedImage.enabled = true;

                button.enabled = false;
                break;
            case ButtonState.unavailable:
                boughtImage.enabled = false;
                lockedImage.enabled = false;
                unavailableImage.enabled = true;

                button.enabled = false;
                break;
        }
    }
}
