using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameManager : MonoBehaviour
{
    private static PreGameManager instance;
    public static PreGameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public PreGameUIManager ui;

    bool foundPartner;
    bool partnerReady;
    bool isReady;

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

        foundPartner = false;
        partnerReady = false;
        isReady = false;
    }

    private void Start()
    {
        if (GameManager.IsMultiplayer)
            LookForPartner();
        else
            ui.Setup();
    }

    private void LookForPartner()
    {
        //Make cool fanzy loading animation
        MultiplayerManager.Instance.RandomMatchMaking();
    }

    public void StartGame()
    {
        if (GameManager.IsMultiplayer)
        {
            if (!partnerReady || !foundPartner || !isReady)
                return;

            if (MultiplayerManager.IsHost)
                MultiplayerManager.SendGameStartToPartner();
        }

        partnerReady = false;
        foundPartner = false;
        isReady = false;

        GameManager.StartGame(); //Start match
    }

    public void ReadyUp()
    {
        isReady = true;
        MultiplayerManager.SendReadyToPartner();
    }

    public void FoundPartner()
    {
        foundPartner = true;
        ui.Setup();
    }

    public void PartnerReady()
    {
        partnerReady = true;
    }
}
