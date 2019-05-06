using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PreGameUIManager : MonoBehaviour
{
    [SerializeField]
    private Text usernameC, partnernameC, usernameS;
    [SerializeField]
    GameObject CoopScreen, SingleplayerScreen, startButtonC;
    [SerializeField]
    Image readyImageC, partnerReadyImageC;
    [SerializeField]
    Sprite whiteReady, greenReady;

    public void Setup()
    {
        switch(GameManager.Gamemode)
        {
            case GameManager.Startmethod.singleplayer:
                SingleplayerScreen.SetActive(true);
                CoopScreen.SetActive(false);
                break;

            case GameManager.Startmethod.multiplayer:
                if (MultiplayerManager.IsHost)
                    startButtonC.SetActive(true);
                else
                    startButtonC.SetActive(false);

                SingleplayerScreen.SetActive(false);
                CoopScreen.SetActive(true);
                partnernameC.text = MultiplayerManager.Instance.PartnerName;
                break;
        }

        usernameC.text = AccountInfo.Instance.DisplayName;
        usernameS.text = AccountInfo.Instance.DisplayName;
    }

    public void ColorReady(bool _ready)
    {
        if (_ready)
        {
            readyImageC.sprite = greenReady;
        }
        else
        {
            readyImageC.sprite = whiteReady;
        }
    }

    public void ColorPartnerReady(bool _ready)
    {
        if (_ready)
        {
            partnerReadyImageC.sprite = greenReady;
        }
        else
        {
            partnerReadyImageC.sprite = whiteReady;
        }
    }
}
