using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PreGameUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startButton, readyButton;
    [SerializeField]
    private Text username, partnername;

    private void Awake()
    {
        switch(GameManager.Gamemode)
        {
            case GameManager.Startmethod.singleplayer:
                startButton.SetActive(true);
                readyButton.SetActive(false);
                username.enabled = true;
                partnername.enabled = false;
                break;

            case GameManager.Startmethod.multiplayer:
                if (MultiplayerManager.IsHost)
                    startButton.SetActive(true);
                else
                    startButton.SetActive(false);

                readyButton.SetActive(true);
                username.enabled = true;
                partnername.enabled = true;

                partnername.text = MultiplayerManager.Instance.PartnerName;
                break;
        }

        username.text = AccountInfo.Instance.DisplayName;
    }
}
