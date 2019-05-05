using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PreGameUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startButton, readyButton;
    [SerializeField]
    private Text usernameC, partnernameC, usernameS;
    [SerializeField]
    GameObject CoopScreen, SingleplayerScreen;

    public void Setup()
    {
        switch(GameManager.Gamemode)
        {
            case GameManager.Startmethod.singleplayer:
                startButton.SetActive(true);
                readyButton.SetActive(false);
                usernameC.enabled = false;
                partnernameC.enabled = false;
                usernameS.enabled = true;

                SingleplayerScreen.SetActive(true);
                CoopScreen.SetActive(false);
                break;

            case GameManager.Startmethod.multiplayer:
                if (MultiplayerManager.IsHost)
                    startButton.SetActive(true);
                else
                    startButton.SetActive(false);

                readyButton.SetActive(true);
                usernameC.enabled = true;
                partnernameC.enabled = true;
                usernameS.enabled = false;

                SingleplayerScreen.SetActive(false);
                CoopScreen.SetActive(true);
                partnernameC.text = MultiplayerManager.Instance.PartnerName;
                break;
        }

        usernameC.text = AccountInfo.Instance.DisplayName;
        usernameS.text = AccountInfo.Instance.DisplayName;
    }
}
